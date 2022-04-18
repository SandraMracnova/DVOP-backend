# DVOP-backend
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
