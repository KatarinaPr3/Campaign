# Campaign
This repository is responsible for managing operations related to the company's reward campaign for loyal customers.  The telecommunication company recently launched a campaign where agents reward loyal customers with discounts.

## Installation

Before you run the code, execute the command
```bash
 dotnet build
```
 then 
```bash
dotnet ef database update
```

After that, it is recommended to run get_all_agents with fill_db = true, as this populates the created database. Agents are generated from the SOAP service with the link defined in the task, based on their type, which is "employee." The fact that not all employees have the same office addresses is disregarded.

The username and password are generated from the names and ID numbers obtained from the soapService.
Example:
### adam_wolfgang_f.96
### wilson_rob_x._101

