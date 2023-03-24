# Budgeting Application

### Introduction
Budgeting application is an open source platform that enable users to run their basic finances and see other content relevant to economics, business 
and crypto world.
### Project Support Features
* Users can signup and login to their accounts
* Public (non-authenticated) users can't access application.
### Installation Guide

### Usage
* Run npm start:dev to start the application.
* Connect to the API using Postman on port 7066.
### API Endpoints
| HTTP Verbs | Endpoints | Action |
| --- | --- | --- |
| POST | /api/user/signup | To sign up a new user account |
| POST | /api/user/login | To login an existing user account |
| POST | /api/causes | To create a new cause |
| GET | /api/causes | To retrieve all causes on the platform |
| GET | /api/causes/:causeId | To retrieve details of a single cause |
| PATCH | /api/causes/:causeId | To edit the details of a single cause |
| DELETE | /api/causes/:causeId | To delete a single cause |
### Technologies Used

### Authors

### License
This project is available for use under the MIT License.
