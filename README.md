# DVOP-backend
Docker Public Hub: 

  - Howto source https://docs.docker.com/samples/dotnetcore/
  - Public Image of dvop_api -> https://hub.docker.com/repository/docker/nalverit/dvop-api
  - docker pull docker.io/nalverit/dvop-api
  - docker run -d -p 5000:5000 docker.io/nalverit/dvop-api

Docker Local Build:
  - Howto source https://docs.docker.com/samples/dotnetcore/
  - cd docker
  - docker-compose build
  - docker-compose up

Visual Code -> Open dvopapi-Mracnova.sln , and build

List of endpoints.
You need to use the token otherwise you will not be able to interact with the endpoints.
* /auth   [POST]

    Authenticates the user (email)
* /add/{token} [POST]
   
   Adds a task (type, numbers, completion status)
* /whoami/{token}  [GET]
   
   Displays the user's email
* /status/{token}  [GET]
   
   Displays how many tasks have been completed/are yet to be completed
* /tasklist/{token}  [GET]
   
   Displays list of tasks
* /process/{token} [GET]
 
   Processes tasks




JSON inputs for various endpoints:

## /auth
```json
{
    "email": "sandra.mracnova@ssps.cz"
}
```
## /add
```json
{
    "taskType": "RECTANGLE-AREA",
    "numberOne": 2,
    "numberTwo": 2,
    "isComplete": false
}
```
