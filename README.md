# todo-list

Simple implementation of the API described [in the link](https://roadmap.sh/projects/todo-list-api).

The following was selected as the main framework ASP.NET. Dapper and PostgreSQL are used to interact with the data (the database schema is automatically pulled up from migration scripts when containers are launched).

### Launching & Usage

To run the project conveniently, you will need an installed Docker client. Run it in the root folder of the project

```
docker-compose build
docker-compose up
```

After the launch of docker, the corresponding API will be available at `http://localhost:5431`. You can go to the `http://localhost:5431/swagger/index.html` to use the swagger interface for interaction. 