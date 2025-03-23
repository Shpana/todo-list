FROM postgres:latest
ADD ["TodoList.Api/Database/Migrations/01_Initialize.sql", "/docker-entrypoint-initdb.d"]
RUN chmod a+r /docker-entrypoint-initdb.d/*
EXPOSE 5432 