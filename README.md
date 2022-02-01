# TrueHomeTest

#Esta es una API creada con .NET 6, que se conecta a una base de datos PostgresSQL

#Utiliza como ORM Dapper

Para poder echar andar el api es necesario que se tenga instalado .NET 6 (Al instalar visual studio community 2022 ya lo tiene por defecto)

#BASE DE DATOS 

Para configurar la coneccion a base de datos es necesario modificar el alchivo "appsettings.json" en la siguiente linea "PostgreSQLConnection": "Server=127.0.0.1;Port=5432;Database=TesTrueHome;User Id=postgres;Password=root;" Y asignar los valores necesarios dependiendo de la cofiguracion que cada quien tenga

*TABLAS PostgressSQL NECESARIAS

PARA QUE LA API FUNCIONE HAY QUE EJECUTAR EL SIGUIENTE SCRIPT

=============INICIA SCRIPT=========================

CREATE TABLE property ( id serial PRIMARY KEY not null, title VARCHAR (255) not null, address TEXT not null, description TEXT not null, created_at TIMESTAMP not null, updated_at TIMESTAMP not null, disabled_at TIMESTAMP, status varchar(35) not null );

create TABLE activity ( id serial PRIMARY KEY not null, property_id int, schedule TIMESTAMP not null, title varchar(255) not null, created_at TIMESTAMP not null, updated_at TIMESTAMP not null, status varchar(35) not null );

create TABLE survey ( id serial PRIMARY KEY not null, activity_id int, answers JSON not null, created_at TIMESTAMP not null );

insert into property(title,address, description, created_at, updated_at, disabled_at, status) values ('Propiedad 1', 'Direccion 1','Descripcion Propiedad 1', now() - INTERVAL '1 DAY', now(), null, 'active'), ('Propiedad 2', 'Direccion 2','Descripcion Propiedad 2', now() - INTERVAL '2 DAY', now(), null, 'inactive'), ('Propiedad 3', 'Direccion 3','Descripcion Propiedad 3', now() - INTERVAL '3 DAY', now(), null, 'active'), ('Propiedad 4', 'Direccion 4','Descripcion Propiedad 4', now() - INTERVAL '4 DAY', now(), null, 'inactive');

=============TERMINA SCRIPT=========================

#Para las validaciones requeridas en el test me tome la libertad de usar los estatus de esta manera 
#(POR LO QUE LA PRUEBA DEL API SE TIENEN QUE ENVIAR CON ESTOS STATUS EN CADENA) 

#status para los "property"
#--(string) active 
#--(string) inactive

#status para los "activity"
#--(string) active
#--(string) cancel 
#--(string) done
