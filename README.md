# Overview
This repository contains tests for comparison of MySql, MariaDb, PostgreSql and Sql Server in docker. It includes single read, update, insert and delete operations. Complex queries are not included in these tests. It also supports commandline arguments.

# Commandline

- *--concurrent* or *-c* : Number of currency, default value is 1, minimum value is 1.
- *--read-pagesize* : Page size for batch reads. This parameter is used in `Update` and `Delete` tests. Default value is 100, minimum value is 1.
- *--read-count* : Number of iterations used in `Read` tests. Default value is 1000, minimum value is 1.
- *--insert-count* : New record count used for `Create` tests. Default value is 1000, minimum values is 1.
- *--insert-pagesize* : Batch size for inserts. Default is 1, minimum value is 1.
- *--databases* or *-d* : List of databases for testing. Tests all databases if it is empty. Recommended way is to test databases one by one. 
Allowed values are `mysql`, `mariadb`, `postgresql`, `sqlserver`. 