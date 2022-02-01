using Microsoft.AspNetCore.Mvc;
using TestTrueHome.Data.Repositories;
using TestTrueHome.Model;
using TestTrueHome.Model.DTO;

namespace TestTrueHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : Controller
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IPropertyRepository _propertyRepository;

        public ActivityController(IActivityRepository activityRepository, IPropertyRepository propertyRepository)
        {
            _activityRepository = activityRepository;
            _propertyRepository = propertyRepository;
        }


        [HttpPost]
        public async Task<IActionResult> createActivity([FromBody] Activity activity)
        {
            if (activity == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();


            Task<Property> propertieStored = _propertyRepository.GetProperty(activity.property_id);
            if (propertieStored == null)
            {
                return BadRequest();
            }
            if (propertieStored.Result == null)
            {
                return BadRequest(String.Format("No existe una propidad con el id {0}", activity.property_id));
            }

            if (propertieStored.Result.status.ToString() == "inactive")
            {
                return BadRequest("No se puede agentar una actividad a esta propidad, por que esta inactiva");
            }

            Task<IEnumerable<Activity>> activitesOfPropertiy = _activityRepository.GetAllActivityOfProperty(activity);

            if (activitesOfPropertiy.Result != null)
            {
                foreach (var activ in activitesOfPropertiy.Result)
                {
                    int result = DateTime.Compare(activ.schedule.AddHours(1), activity.schedule);
                    if (result == 0)
                        return BadRequest("Ya hay agendada una cita para esta propiedad a la misma hora");
                }
            }
            var created = await _activityRepository.InsertActivity(activity);
            return Created("created", created);
        }

        [HttpPut("reagendar")]
        public async Task<IActionResult> ReagendarActivity([FromBody] Activity activity)
        {
            if (activity == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();


            var activityStored = _activityRepository.GetActivityById(activity.Id);

            if (activityStored.Result == null) {
                return NotFound(String.Format("No existe una actividad con el id {0}", activity.Id));
            }

            if (activityStored.Result.status == "cancel")
            {

                return BadRequest("No se pude reagendar la activadad por que ya ha sido cancelada");
            }

            Task<IEnumerable<Activity>> activitesOfPropertiy = _activityRepository.GetAllActivityOfProperty(activity);



            if (activitesOfPropertiy.Result != null)
            {
                foreach (var activ in activitesOfPropertiy.Result)
                {
                    if (activ.Id != activity.Id) // valida que no sea la misma actividad
                    {
                        int result = DateTime.Compare(activ.schedule.AddHours(1), activity.schedule);
                        if (result == 0)
                            return BadRequest("Ya hay agendada una cita para esta propiedad a la misma hora");
                    }

                }
            }

            bool resul = await _activityRepository.ReagendarActividad(activity);



            return (resul) ? Ok("Se reagendo la actividad") : BadRequest();
        }

        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> cancelarActivity(int id)
        {

            var activityStored = _activityRepository.GetActivityById(id);

            if (activityStored.Result == null)
                return NotFound(String.Format("No existe una actividad con el id {0}", id));


            if (activityStored.Result.status == "cancel")
                return BadRequest("La actividad ya estaba cancelada cancelada");


            bool resul = await _activityRepository.CancelarActivity(id);

            return (resul) ? Ok("Actividad cancelada") : BadRequest();
        }

        [HttpGet("listaActiviades")]
        public async Task<IActionResult> getListaActivity(DateTime? d1, DateTime? d2, string? status)
        {

            string? dt1 = (d1 == null) ? null : String.Format("{0}-{1}-{2}", d1.Value.Year, (d1.Value.Month < 10) ? "0" + d1.Value.Month : d1.Value.Month, (d1.Value.Day < 10) ? "0" + d1.Value.Day : d1.Value.Day);
            string? dt2 = (d2 == null) ? null : String.Format("{0}-{1}-{2}", d2.Value.Year, (d2.Value.Month < 10) ? "0" + d2.Value.Month : d2.Value.Month, (d2.Value.Day < 10) ? "0" + d2.Value.Day : d2.Value.Day); ;
            

            Task<IEnumerable<Activity>> activitiesList = _activityRepository.GetListOfActivities(dt1, dt2, status);
            if (activitiesList.Result == null)
                return Ok("No hay actividades");


            List<ActivityToList> listToSend = new List<ActivityToList>();
            foreach (var a in activitiesList.Result)
            {

                var resultDate = DateTime.Compare(a.schedule, DateTime.Now);
                var condition = "";
                if (a.status == "active")
                {
                    if (resultDate > 0 || resultDate == 0)
                        condition = "Pendiente a realizar";
                    else
                        condition = "Atrasada";
                }
                else if (a.status == "done")
                {
                    condition = "Finalizada";
                }
                else 
                {
                    condition = "Cancelada";
                }

                var property = _propertyRepository.GetProperty(a.property_id);
                

                listToSend.Add(new ActivityToList
                {
                    Id = a.Id,
                    schedule = a.schedule,
                    title = a.title,
                    created_at = a.created_at,
                    status = a.status,
                    condition = (condition != "") ? condition : "Sin Información",
                    property = new PropertyDTO()
                    {
                        id = property.Result != null ? property.Result.id : -1,
                        title = property.Result != null ? property.Result.title : "Sin Información",
                        address = property.Result != null ? property.Result.address : "Sin Información"
                    },
                    survey = new SurveyDTO() 
                    { 
                        id = -1,
                        activity_id = -1,
                        answers = "[{ answer_1: 'Text Dummy', answer_1: 'Text Dummy'},{ answer_1: 'Text Dummy', answer_1: 'Text Dummy'}]"

                    }

                });
            }

            return Ok(listToSend);
        }




    }
}
