### What is Tankman?

Tankman is a service which offers a REST API to do the following:

- Manage organizations, roles, users, resources and permissions
- Assign roles to users
- Assign user permissions and role permissions to various resources
- Attach custom properties to Orgs, Roles, Users and query them

It's important to note that Tankman does not function as an Identity and Authentication server. However, it can work well with popular open-source identity solutions such as [Ory Kratos](https://github.com/ory/kratos), various OAuth-based providers, or even your own custom authentication mechanism.

Tankman is available on GitHub under the MIT license. You can download it as a binary for your platform; and it utilizes PostgreSQL as its underlying database.
Better formatted documentation can be found on the [Tankman Website - tankman.dev](https://tankman.dev).

### Install Tankman

Download binaries for your platform
- For most Linux distros [Linux x86-64](https://github.com/lesser-app/tankman/releases/download/tankman-1.0.0-beta1/tankman-linux-x64) 
- For Linux distros using musl [Linux x86-64](https://github.com/lesser-app/tankman/releases/download/tankman-1.0.0-beta1/tankman-linux-musl-x64) 
- For ARM 64 bit Linux distros [Linux ARM64](https://github.com/lesser-app/tankman/releases/download/tankman-1.0.0-beta1/tankman-linux-arm64)
- [macOS M1](https://github.com/lesser-app/tankman/releases/download/tankman-1.0.0-beta1/tankman-osx13-arm64)
- [macOS x64](https://github.com/lesser-app/tankman/releases/download/tankman-1.0.0-beta1/tankman-osx-x64)
- [Windows x86-64 (untested)](https://github.com/lesser-app/tankman/releases/download/tankman-1.0.0-beta1/tankman-win-x64.exe)

### Configuring Tankman

Step 1: Create a new database for tankman on your PostgreSQL server. Call it whatever you want, but most people call it tankmandb.

Step 2: Initialize the database. You can do that with the following command:

```sh
./tankman --initdb --dbhost YOUR_PG_HOST --dbport YOUR_PG_PORT --dbuser YOUR_PG_USER --dbpass YOUR_PG_PASSWORD
```

That's it. You're ready to roll.

### Running Tankman

The following command will start tankman on localhost:1989

```sh
./tankman --dbhost YOUR_PG_HOST --dbport YOUR_PG_PORT --dbuser YOUR_PG_USER --dbpass YOUR_PG_PASSWORD
```

You can change the hostname and port with the `--host` and `--port` CLI options.

```sh
./tankman --host 127.0.0.1 --port 1990 --dbhost YOUR_PG_HOST --dbport YOUR_PG_PORT --dbuser YOUR_PG_USER --dbpass YOUR_PG_PASSWORD
```

### Configuring via Environment Variables

Instead of using CLI options as given above, you may use $TANKMAN_HOST (instead of `--host`), $TANKMAN_PORT (instead of `--port`) and $TANKMAN_CONN_STR (instead of `--dbhost`, `--dbport`, `--dbuser`, `--dbpass`) environment variables.

Like this:

```sh
TANKMAN_HOST=localhost
TANKMAN_PORT=1990
TANKMAN_CONN_STR=Server=localhost;Port=5432;Database=tankmandb;User Id=postgres;Password=postgres

# run tankman!
./tankman
```

### ⚠️ Caution 

Tankman is an internal service which should be accessible only from your backend apps. Make sure that you don't expose Tankman ports publicly.

## Organizations

Organizations are at the root of the entity hierarchy, and should be the first thing you should create. You may create as many orgs are you need.

### Create an Organization

```tpl
HTTP POST /orgs
```

Payload:

```json
{
  "id": "example.com",
  "data": "data for example.com"
}
```

Response:

```json
{
  "data": {
    "id": "example.com",
    "createdAt": "2023-07-02T01:32:18.4557911Z",
    "data": "data for example.com",
    "properties": {}
  }
}
```

### Get Organizations

```tpl
HTTP GET /orgs
```

Response:

```json
{
  "data": [
    {
      "id": "example.com",
      "createdAt": "2023-07-02T01:38:18.764642Z",
      "data": "data for example.com",
      "properties": {}
    },
    {
      "id": "agilehead.com",
      "createdAt": "2023-07-01T01:38:18.764642Z",
      "data": "some other data",
      "properties": {}
    }
  ]
}
```

Pagination is done with the `from` and `limit` query parameters.

```tpl
HTTP GET /orgs?from=10&limit=100
```

To get a single organization (as a list with one item), specify the Org's id.

```tpl
HTTP GET /orgs/{orgId}

# For example
HTTP GET /orgs/example.com
```

You can specify multiple Org ids.

```tpl
HTTP GET /orgs/{orgId1,orgId2}

# For example
HTTP GET /orgs/example.com,northwind
```

### Update an Organization

```tpl
HTTP PUT /orgs/{orgId}

# For example:
HTTP PUT /orgs/example.com
```

Payload:

```json
{
  "data": "new data for example.com"
}
```

Response:

```json
{
  "data": {
    "id": "example.com",
    "createdAt": "2023-07-02T01:38:18.764642Z",
    "data": "new data for example.com",
    "properties": {}
  }
}
```

### Delete an Organization

Deleting an organization will delete everything associated with it - resources, roles, users, permissions etc. This is indeed a very destructive operation.

```tpl
HTTP DELETE /orgs/{orgId}

# For example:
HTTP DELETE /orgs/example.com
```

Since it can cause a lot of damage, there's an CLI option to require a safetyKey for deleting organizations.

You need to start tankman like this:

```sh
./tankman --safety-key $SAFETY_KEY

# For example:
./tankman --safety-key NOFOOTGUN
```

With the `--safety-key` option, HTTP DELETE should specify the safetyKey parameter as a query string.

```tpl
HTTP DELETE /orgs/{orgId}?safetyKey=$SAFETY_KEY

# For example
HTTP DELETE /orgs/example.com?safetyKey=NOFOOTGUN
```

### Add a Custom Property

You can add custom string properties to an Organization entity.

```tpl
HTTP PUT /orgs/{orgId}/properties/{propertyName}

For example:
HTTP PUT /orgs/example.com/properties/country
```

Payload:

```json
{
  "value": "India"
}
```

Response:

```json
{
  "data": {
    "name": "country",
    "value": "India",
    "hidden": false,
    "createdAt": "2023-07-02T02:19:15.499711Z"
  }
}
```

When properties are added to an Org, they are returned when the Org is fetched.

For example, `GET /orgs/example.com` will retrieve the following response. Note the added properties object.

```json
{
  "data": [
    {
      "id": "example.com",
      "createdAt": "2023-07-02T01:38:18.764642Z",
      "data": "data for example.com",
      "properties": {
        "country": "India"
      }
    }
  ]
}
```

### Get the value of a property

Properties are usually read as a part of the fetching entity; Org in this case. But if you need to get a list of properties without fetching the entity you can use the following API.

```tpl
HTTP GET /orgs/{orgId}/properties/{propertyName}

# For example:
HTTP GET /orgs/example.com/properties/country
```

Response:

```json
{
  "data": [
    {
      "name": "country",
      "value": "India",
      "hidden": false,
      "createdAt": "2023-07-02T02:19:15.499711Z"
    }
  ]
}
```

### Delete a Custom Property

```tpl
HTTP DELETE /orgs/{orgId}/properties/{propertyName}

# For example:
HTTP DELETE /orgs/example.com/properties/country
```

### Creating Hidden Properties

When a property is hidden, it will not be included when the organization is fetched. To create a hidden property, add the hidden flag when creating the property.

To create a Hidden Property:

```tpl
HTTP PUT /orgs/{orgId}/properties/{propertyName}

# For example:
HTTP PUT /orgs/example.com/properties/revenue
```

Payload should include the `hidden` attribute:

```json
{
  "value": "2340000",
  "hidden": true
}
```

To include a hidden property when querying Orgs, it should be explicitly mentioned. Note that properties which aren't hidden are always included.

```tpl
HTTP GET /orgs?properties={propertyName}

For example:
HTTP GET /orgs?properties={revenue}
```

Response:

```json
{
  "data": [
    {
      "id": "example.com",
      "createdAt": "2023-07-02T01:38:18.764642Z",
      "data": "new data for example.com",
      "properties": {
        "country": "India",
        "revenue": "2340000"
      }
    }
  ]
}
```

### Filtering by Property

Organizations may be filtered by the custom property.

```tpl
HTTP GET /orgs?properties.{propertyName}={propertyValue}

# For example, fetch orgs with country = India
HTTP GET /orgs?properties.country=India
```

## Roles

Roles belong to an Organization. Users can be assigned Roles. Roles can have Permissions to various Resources.

### Create a Role

```tpl
HTTP POST /orgs/{orgId}/roles

# For example:
HTTP POST /orgs/example.com/roles
```

Payload:

```json
{
	"id": "admins",
	"data": "data for admins"
}
```

Response:

```json
{
	"data": {
		"id": "admins",
		"data": "data for admins",
		"createdAt": "2023-07-02T08:31:59.7931788Z",
		"orgId": "example.com",
		"properties": {}
	}
}
```

### Get Roles

```tpl
HTTP GET /orgs/{orgId}/roles/

For example:
HTTP GET /orgs/example.com/roles/
```

Response:

```json
{
	"data": [
		{
			"id": "admins",
			"data": "data for admins",
			"createdAt": "2023-06-30T17:01:13.525215Z",
			"orgId": "agilehead.com",
			"properties": {
				"prop1": "value1"
			}
		},
		{
			"id": "devs",
			"data": "data for devs",
			"createdAt": "2023-07-01T11:56:04.039094Z",
			"orgId": "agilehead.com",
			"properties": {}
		}
	]
}
```

To get a single role (as a list with one item), specify the Role's id.

```tpl
HTTP GET /orgs/{orgId}/roles/{roleId}

# For example
HTTP GET /orgs/example.com/roles/admins
```

You can specify multiple Role ids.

```tpl
HTTP GET /orgs/{orgId}/roles/{roleId}

# For example
HTTP GET /orgs/example.com/roles/admins,devops
```


### Update a Role

```tpl
HTTP PUT /orgs/{orgId}/roles/{roleId}

# For example:
HTTP PUT /orgs/example.com/roles/admins
```

Payload:

```json
{
	"data": "new data for admins"
}
```

Response:

```json
{
	"data": {
		"id": "admins",
		"data": "new data for admins",
		"createdAt": "2023-07-02T08:31:59.793178Z",
		"orgId": "example.com",
		"properties": {}
	}
}
```

### Delete a Role

Deleting a role will also delete associated permissions.

```tpl
HTTP DELETE /orgs/{orgId}/roles/{roleId}

# For example:
HTTP DELETE /orgs/example.com/roles/admins
```

### Add a Custom Property

You can add custom string properties to an Role entity.

```tpl
HTTP PUT: /orgs/{orgId}/roles/{roleId}/properties/{propertyName}

For example:
HTTP PUT: /orgs/example.com/roles/admins/properties/privileged
```

Payload:

```json
{
	"value": "yes"
}
```

Response:

```json
{
	"data": {
		"name": "country",
		"value": "India",
		"hidden": false,
		"createdAt": "2023-07-02T02:19:15.499711Z"
	}
}
```

When properties are added to an Role, they are returned when the Role is fetched.

For example, `GET /orgs/example.com/roles/admins` will retrieve the following response. Note the added properties object.

```json
{
	"data": [
		{
			"id": "example.com",
			"createdAt": "2023-07-02T01:38:18.764642Z",
			"data": "data for admins",
			"properties": {
				"privileged": "yes"
			}
		}
	]
}
```

### Get the value of a property

Properties are usually read as a part of the fetching entity; Role in this case. But if you need to get a list of properties without fetching the entity you can use the following API.

```tpl
HTTP GET /orgs/{orgId}/roles/{roleId}/properties/{propertyName}

# For example:
HTTP GET /orgs/example.com/properties/roles/roles/admins/privileged
```

Response:

```json
{
	"data": [
		{
			"roleId": "admins",
			"orgId": "example.com",
			"name": "privileged",
			"value": "yes",
			"hidden": false,
			"createdAt": "2023-07-02T12:31:01.978925Z"
		}
	]
}
```


### Delete a Custom Property 

```tpl
HTTP DELETE: /orgs/{orgId}/roles/{roleId}/properties/{propertyName}

# For example:
HTTP DELETE: /orgs/example.com/roles/admins/properties/privileged
```

### Creating Hidden Properties

When a property is hidden, it will not be included when the Role is fetched. To create a hidden property, add the hidden flag when creating the property.

To create a Hidden Property:

```tpl
HTTP PUT: /orgs/{orgId}/roles/{roleId}/properties/{propertyName}

# For example:
HTTP PUT: /orgs/example.com/roles/admins/properties/active
```

Payload should include the `hidden` attribute:

```json
{
  "active": "yes",
  "hidden": true
}
```

To include a hidden property when querying Roles, it should be explicitly mentioned. Note that properties which aren't hidden are always included.

```tpl
HTTP GET /orgs/{orgId}/roles?properties={propertyName}

For example:
HTTP GET /orgs/example.com/roles?properties=active
```

Response:

```json
{
	"data": [
		{
			"id": "admins",
			"data": "data for admins",
			"createdAt": "2023-07-02T12:30:55.981226Z",
			"orgId": "example.com",
			"properties": {
				"active": "yes",
				"privileged": "yes"
			}
		}
	]
}
```

### Filtering by Property

Roles may be filtered by the custom property.

```tpl
HTTP GET /orgs/{orgId}/roles?properties.{propertyName}={propertyValue}

# For example, fetch roles with active = yes
HTTP GET /orgs/example.com/roles?properties.active=yes
```

### Finds users in a Role

See a list of users who have a specific Role.

```tpl
HTTP GET /orgs/{orgId}/roles/{roleId}/users

# For example, fetch roles with active = yes
HTTP GET /orgs/example.com/roles/admins/users
```

## Users

Users belong to an organization. Users may belong to Roles, and can have Permissions to various Resources.

### Create a User

```tpl
HTTP POST /orgs/{orgId}/users

# For example:
HTTP POST /orgs/example.com/users
```

Payload:

```json
{
	"id": "user3",
	"identityProviderUserId": "jeswinpk@agilehead.com",
	"identityProvider": "Google",
	"data": "data for aser3"
}
```

Response:

```json
{
	"data": {
		"roleIds": [],
		"properties": {},
		"id": "user3",
		"data": "data for aser3",
		"identityProvider": "Google",
		"identityProviderUserId": "jeswinpk@agilehead.com",
		"createdAt": "2023-07-06T13:01:26.6594794Z",
		"orgId": "example.com"
	}
}
```

### Get Users

```tpl
HTTP GET /orgs/{orgId}/users/

For example:
HTTP GET /orgs/example.com/users/
```

Response:

```json
{
	"data": [
		{
			"roleIds": [],
			"properties": {},
			"id": "user3",
			"data": "data for aser3",
			"identityProvider": "Google",
			"identityProviderUserId": "jeswinpk@agilehead.com",
			"createdAt": "2023-07-06T13:01:26.659479Z",
			"orgId": "example.com"
		}
	]
}
```

To get a single user (as a list with one item), specify the user's id.

```tpl
HTTP GET /orgs/{orgId}/users/{userId}

# For example
HTTP GET /orgs/example.com/users/user3
```

You can specify multiple user ids.

```tpl
HTTP GET /orgs/{orgId}/users/{userId}

# For example
HTTP GET /orgs/example.com/users/user3,user5
```


### Update a user

```tpl
HTTP PUT /orgs/{orgId}/users/{userId}

# For example:
HTTP PUT /orgs/example.com/users/user3
```

Payload:

```json
{
	"identityProviderUserId": "jeswinpk@agilehead.com",
	"identityProvider": "Google",
	"data": "new data for user3"
}
```

Response:

```json
{
	"data": {
		"roleIds": [],
		"properties": {},
		"id": "user3",
		"data": "new data for user3",
		"identityProvider": "Google",
		"identityProviderUserId": "jeswinpk@agilehead.com",
		"createdAt": "2023-07-06T13:01:26.659479Z",
		"orgId": "example.com"
	}
}
```

### Delete a user

Deleting a user will also delete associated user permissions.

```tpl
HTTP DELETE /orgs/{orgId}/users/{userId}

# For example:
HTTP DELETE /orgs/example.com/users/user3
```

### Assiging Roles

```tpl
HTTP POST /orgs/{orgId}/users/{userId}/roles

# For example:
HTTP DELETE /orgs/example.com/users/user3/roles
```

Payload:

```json
{
	"roleId": "admins"
}
```

Response:

```json
{
	"data": {
		"userId": "user3",
		"roleId": "admins",
		"createdAt": "2023-07-01T11:57:54.0264874Z",
		"orgId": "agilehead.com"
	}
}
```

### Unassigning Roles

```tpl
HTTP DELETE /orgs/{orgId}/users/{userId}/roles/{roleId}

# For example:
HTTP DELETE /orgs/example.com/users/user3/roles/admins
```

### Finds users in a Role

See a list of users who have a specific Role.

```tpl
HTTP GET /orgs/{orgId}/roles/{roleId}/users

# For example, fetch roles with active = yes
HTTP GET /orgs/example.com/roles/admins/users
```

### Add a Custom Property

You can add custom string properties to an user entity.

```tpl
HTTP PUT: /orgs/{orgId}/users/{userId}/properties/{propertyName}

For example:
HTTP PUT: /orgs/example.com/users/user3/properties/firstName
```

Payload:

```json
{
	"value": "Jeswin"
}
```

Response:

```json
{
	"data": {
		"name": "firstName",
		"value": "Jeswin",
		"hidden": false,
		"createdAt": "2023-07-06T13:07:41.8782012Z"
	}
}
```

When properties are added to an user, they are returned when the user is fetched.

For example, `GET /orgs/example.com/users/user3` will retrieve the following response. Note the added properties object.

```json
{
	"data": [
		{
			"roleIds": [],
			"properties": {
				"firstName": "Jeswin"
			},
			"id": "user3",
			"data": "new data for user3",
			"identityProvider": "Google",
			"identityProviderUserId": "jeswinpk@agilehead.com",
			"createdAt": "2023-07-06T13:01:26.659479Z",
			"orgId": "example.com"
		}
	]
}
```

### Get the value of a property

Properties are usually read as a part of the fetching entity; user in this case. But if you need to get a list of properties without fetching the entity you can use the following API.

```tpl
HTTP GET /orgs/{orgId}/users/{userId}/properties/{propertyName}

# For example:
HTTP GET /orgs/example.com/properties/users/users/user3/firstName
```

Response:

```json
{
	"data": [
		{
			"userId": "user3",
			"orgId": "example.com",
			"name": "firstName",
			"value": "Jeswin",
			"hidden": false,
			"createdAt": "2023-07-06T13:07:41.878201Z"
		}
	]
}
```


### Delete a Custom Property 

```tpl
HTTP DELETE: /orgs/{orgId}/users/{userId}/properties/{propertyName}

# For example:
HTTP DELETE: /orgs/example.com/users/user3/properties/firstName
```

### Creating Hidden Properties

When a property is hidden, it will not be included when the user is fetched. To create a hidden property, add the hidden flag when creating the property.

To create a Hidden Property:

```tpl
HTTP PUT: /orgs/{orgId}/users/{userId}/properties/{propertyName}

# For example:
HTTP PUT: /orgs/example.com/users/user3/properties/active
```

Payload should include the `hidden` attribute:

```json
{
  "active": "yes",
  "hidden": true
}
```

To include a hidden property when querying users, it should be explicitly mentioned. Note that properties which aren't hidden are always included.

```tpl
HTTP GET /orgs/{orgId}/users?properties={propertyName}

For example:
HTTP GET /orgs/example.com/users?properties=active
```

Response:

```json
{
	"data": [
		{
			"roleIds": [],
			"properties": {
				"active": "yes",
				"firstName": "Jeswin"
			},
			"id": "user3",
			"data": "new data for user3",
			"identityProvider": "Google",
			"identityProviderUserId": "jeswinpk@agilehead.com",
			"createdAt": "2023-07-06T13:01:26.659479Z",
			"orgId": "example.com"
		}
	]
}
```

### Filtering by Property

users may be filtered by the custom property.

```tpl
HTTP GET /orgs/{orgId}/users?properties.{propertyName}={propertyValue}

# For example, fetch users with active = yes
HTTP GET /orgs/example.com/users?properties.active=yes
```

Response:

```json
{
	"data": [
		{
			"roleIds": [],
			"properties": {
				"firstName": "Jeswin"
			},
			"id": "user3",
			"data": "new data for user3",
			"identityProvider": "Google",
			"identityProviderUserId": "jeswinpk@agilehead.com",
			"createdAt": "2023-07-06T13:01:26.659479Z",
			"orgId": "example.com"
		}
	]
}
```

## Resources

Resources belong to Organizations - and they're names of various entities in the Organization. We use Unix-like paths to represent entities in an organization.

Let's see some examples.

Here's how you'd store some file paths.

- /files/legal/abc.doc
- /files/marketing/another.html

But anything can be a path. For example, if you wanted to control access to a feature, you could define a path as follows and check a user's permissions to it.

- /features/sellbitcoin
- /features/bulkbuy

### Create a Resource

```tpl
HTTP POST /orgs/{orgId}/resources

For example:
HTTP POST /orgs/example.com/resources
```

Payload:

```json
{
  "id": "/root/drives/c/home",
  "data": "data for /root/drives/c/home"
}
```

Response:

```json
{
  "data": {
    "id": "/root/drives/c/home",
    "data": "data for /root/drives/c/home",
    "createdAt": "2023-07-06T13:40:03.4217161Z",
    "orgId": "example.com"
  }
}
```

### Get Resources

```tpl
HTTP GET /orgs/{orgId}/resources

For example:
HTTP GET /orgs/example.com/resources
```

Response:

```json
{
  "data": [
    {
      "id": "/root/drives/c/home",
      "data": "data for /root/drives/c/home",
      "createdAt": "2023-07-06T13:40:03.421716Z",
      "orgId": "example.com"
    }
  ]
}
```

Pagination is done with the `from` and `limit` query parameters.

```tpl
HTTP GET /orgs/example.com/resources?from=10&limit=100
```

To get a single resource (as a list with one item), specific the Resource's path.

```tpl
HTTP GET /orgs/{orgId}/resources/{resourcePath}

# For example
HTTP GET /orgs/example.com/resources/root/drives/c/home
```

### Wildcard searches

A wildcard search allows you to search for all resources starting with a certain path. The wildcard character to use is a tilde(~).

```tpl
HTTP GET /orgs/{orgId}/resources/{basePath}/~

# For example
HTTP GET /orgs/example.com/resources/root/drives/~
```

Assume you have the following resources:

- /orgs/example.com/resources/root/drives/c/home
- /orgs/example.com/resources/root/drives/d/home

The request `HTTP GET /orgs/example.com/resources/root/drives/~` will fetch both since they start with `/root/drives/`.

### Update a Resource

```tpl
HTTP PUT /orgs/{orgId}/resources/{resourcePath}

For example:
HTTP PUT /orgs/example.com/resources/root/drives/c/home
```

Payload:

```json
{
  "data": "new data for /root/drives/c/home"
}
```

Response:

```json
{
  "id": "/root/drives/c/home",
  "data": "new data for /root/drives/c/home",
  "createdAt": "2023-06-30T13:39:29.182724Z",
  "orgId": "agilehead.com"
}
```

### DELETE a Resource

```tpl
HTTP DELETE /orgs/{orgId}/resources/{resourcePath}

For example:
HTTP DELETE /orgs/example.com/resources/root/drives/c/home
```

## Permissions

Permissions are entities which determine whether a Role or a User has access to a Resource. Permissions also have an "action" property which specifies what the Role or User is allowed to do with the Resource.

### Creating a Role Permission

```tpl
HTTP POST /orgs/{orgId}/roles/{roleId}/permissions

For example:
HTTP POST /orgs/example.com/roles/admins/permissions
```

Payload:

```json
{
	"resourceId": "/root/drives/c/home",
	"roleId": "admins",
	"action": "write"
}
```

Response:

```json
{
	"roleId": "admins",
	"resourceId": "/root/drives/c/home",
	"action": "write",
	"createdAt": "2023-06-30T14:04:17.919562Z",
	"orgId": "agilehead.com"
}
```

The example above specifies that the role "admins" can "write" to the resource "/root/drives/c/home". Note that the resource should already exist. 


### Creating a User Permission

```tpl
HTTP POST /orgs/{orgId}/users/{userId}/permissions

For example:
HTTP POST /orgs/example.com/users/user3/permissions
```

Payload:

```json
{
	"resourceId": "/root/drives/c/home",
	"userId": "user3",
	"action": "read"
}
```

Response:

```json
{
	"userId": "admins",
	"resourceId": "/root/drives/c/home",
	"action": "write",
	"createdAt": "2023-06-30T14:04:17.919562Z",
	"orgId": "agilehead.com"
}
```

The example above specifies that the user "user3" can "read" the resource "/root/drives/c/home".

### Getting Effective Permissions for a User

Effective Permissions for a user is the combined list of all user permissions defined for the specifc user, and role permissions for the roles in which the user is a member.

For example, if the user "john" is a member of roles "admins" and "devops", effective permissions to a resource is the combined list of john's permissions to the specific resource, the role "admins" permissions to the resource, and the role "devops" permissions to the resource.

```tpl
HTTP GET /orgs/{orgId}/users/{userId}/effective-permissions/{action}/{resourceId}
For example:
HTTP GET /orgs/example.com/users/user3/effective-permissions/write/root/drives/c/home
```

Response:

```json
[
	{
		"roleId": "admins",
		"resourceId": "/root/drives/c/home",
		"action": "write",
		"createdAt": "2023-06-30T14:04:17.919562Z",
		"orgId": "agilehead.com"
	}
]
```


### Wildcard actions

You can specify a wildcard (tilde "~", by default) to get all permissions irrespective of action.

For example, the following will fetch read and write actions:

```tpl
HTTP GET /orgs/example.com/users/user3/effective-permissions/~/root/drives/c/home
```

Response:

```json
[
	{
		"userId": "user3",
		"resourceId": "/root/drives/c/home",
		"action": "read",
		"createdAt": "2023-06-30T14:04:01.837162Z",
		"orgId": "agilehead.com"
	},
	{
		"roleId": "admins",
		"resourceId": "/root/drives/c/home",
		"action": "write",
		"createdAt": "2023-06-30T14:04:17.919562Z",
		"orgId": "agilehead.com"
	}
]
```

### Wildcard Resource Paths

You can specify a wildcard in the resource path as well.

For example, the following will return all resources starting with "/root/drives". Note that we've used a wild card action as well.

```tpl
HTTP GET /orgs/example.com/users/user3/effective-permissions/~/root/drives/~
```

### Getting just Role Permissions

You can get permissions for a role to a resource thus.

```tpl
HTTP GET /orgs/{orgId}/roles/{roleId}/permissions

For example:
HTTP GET /orgs/example.com/roles/admins/permissions
```

Response:

```json
{
	"data": {
		"roleId": "admins",
		"resourceId": "/root/drives/c/home",
		"action": "write",
		"createdAt": "2023-07-06T14:35:31.3918132Z",
		"orgId": "example.com"
	}
}
```

### Getting just User Permissions

You can get permissions for a user to a resource thus.

```tpl
HTTP GET /orgs/{orgId}/users/{userId}/permissions

For example:
HTTP GET /orgs/example.com/users/user3/permissions
```

Response:

```json
{
	"data": {
		"userId": "user3",
		"resourceId": "/root/drives/c/home",
		"action": "read",
		"createdAt": "2023-07-06T14:35:31.3918132Z",
		"orgId": "example.com"
	}
}
```

### Deleting Role Permissions

You can delete permissions for a role to a resource thus.

```tpl
HTTP DELETE /orgs/{orgId}/roles/{roleId}/permissions/{action}/{resourceId}

For example:
HTTP DELETE /orgs/example.com/roles/admins/permissions/read/root/drives/c/home
```

Response:

```json
{
	"data": {
		"roleId": "admins",
		"resourceId": "/root/drives/c/home",
		"action": "write",
		"createdAt": "2023-07-06T14:35:31.3918132Z",
		"orgId": "example.com"
	}
}
```

### Deleting User Permissions

You can delete permissions for a user to a resource thus.

```tpl
HTTP DELETE /orgs/{orgId}/users/{userId}/permissions/{action}/{resourceId}

For example:
HTTP DELETE /orgs/example.com/users/admins/permissions/read/root/drives/c/home
```

Response:

```json
{
	"data": {
		"userId": "admins",
		"resourceId": "/root/drives/c/home",
		"action": "write",
		"createdAt": "2023-07-06T14:35:31.3918132Z",
		"orgId": "example.com"
	}
}
```

# Help Wanted

1. Test Suite. We need to build a full test suite. This is our highest priority item.
2. Publish to Docker Hub
3. Docker compose file with PostgreSQL
4. Clean up the Documentation
5. Document patterns to use for Authorization.


