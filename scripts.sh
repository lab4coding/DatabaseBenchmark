# Run Postgres db
docker run --name postgres -e POSTGRES_PASSWORD=password -p 5433:5432 -v postgres_data:/var/lib/postgresql/data -d postgres:alpine

# Postgres Create database
docker exec -it postgres bash
psql -U postgres -h localhost postgres
CREATE DATABASE db_benchmark WITH ENCODING 'UTF8';
\l
\q
exit

# Run Mysql db
docker run --name mysql -e MYSQL_ROOT_PASSWORD=password -p 3306:3306 -v mysql_data:/var/lib/mysql -d mysql:latest

# Mysql Create DATABASE
docker exec -it mysql bash
mysql -uroot -ppassword
CREATE DATABASE db_benchmark;
SHOW DATABASES;
EXIT;
exit

# Run Mariadb
docker run --name mariadb -e MYSQL_ROOT_PASSWORD=password -p 3307:3306 -v mariadb_data:/var/lib/mysql -d mariadb:latest

# MariaDB Create Database
docker exec -it mariadb bash
mysql -uroot -ppassword
CREATE DATABASE db_benchmark;
SHOW DATABASES;
EXIT;
exit

# Run Sql server
docker run --name sqlserver -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=passw0rd.' -p 1433:1433 -v sqlserver_data:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2019-latest

# Sql Server Create Database
docker exec -it sqlserver bash
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'passw0rd.'
CREATE DATABASE db_benchmark
GO
select name from sys.databases
GO
:EXIT
exit