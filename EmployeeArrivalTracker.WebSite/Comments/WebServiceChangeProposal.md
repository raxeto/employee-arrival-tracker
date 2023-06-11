# Web service architectural change proposal
The functions that the service needs to perform are two - subscribing for data reception and periodically receiving data. 
It is a good idea to separate them into two distinct functionalities, not tied to each other, even as microservices that perform the two functions. 
The subscription for data reception should be stored in some kind of database so that if there is an interruption in any of the services, 
the data submission can be restored. Splitting into two microservices also has the advantage that subscription is expected to occur much less frequently than data transmission.
This way, different server resources can be allocated for the two services based on their workload.