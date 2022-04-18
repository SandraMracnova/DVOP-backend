# DVOP-backend
List of endpoints.
You need to use the token otherwise you will not be able to interact with the endpoints.

* /add/{token}
   
   Adds a task (type, numbers, completion status)
* /auth
   
   Authenticates the user (email)
* /whoami/{token}
   
   Displays the user's email
* /status/{token}
   
   Displays how many tasks have been completed/are yet to be completed
* /tasklist/{token}
   
   Displays list of tasks
* /process/{token}
 
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
