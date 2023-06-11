# Employee arrivals website

### Employees

I seeded employees in the database from your employees.json file in EmployeeArrivalWriteService.

### Pages
For testing purposes a subscription page (Subscribe) has been created, where by selecting a date, a request is sent to your test service (WebService project). Otherwise, in a real environment, this subscription should happen periodically when the previous token expires. 

Arrivals report page shows stored information about arrivals.

### How to run the project
1. Change default connection string to point to your test sql server in the following files
SubscribeEmployeeArrivalService\appsettings.json
EmployeeArrivalReadService\appsettings.json
EmployeeArrivalWriteService\appsettings.json

2. In package manager console run update-database for EmployeeArrivalReadService

3. In package manager console run update-database for SubscribeEmployeeArrivalService

4. Start projects

5. Start your WebService project

5. Go to Subscribe page and make new subscription

6. After a while check received arrivals in Arrivals Report page

### Tasks for future consideration
The communication between the different internal microservices is not fully cleared up; it needs to be considered how they would best communicate (with a message queue) or in another way. 

Also, at the moment, EmployeeArrivalReadService and EmployeeArrivalWriteService are working on the same database. The idea is for EmployeeArrivalWriteService to continue working on it, the database itself to be replicated in read-only mannar, and EmployeeArrivalReadService to read from it. This way, the three inner microservices in the application will be isolated and independent from one another.

Arrivals report page should support pagination

Token database might be key-value

Add unit testing for all projects