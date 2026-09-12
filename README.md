# CLDV6212 POE Part 1 - CoffeeNChill

## Team Members 

- Muhammad Naveed
- Luthando Mtshali
- Nathan Beaumont
- Coherence Mlambo

## Member 1 - Muhammad Naveed

## Responsiblity

I was responsible for developing the Azure Table Storage foundation and 
the initial menu management functionality for the CoffeeNChill system.

## Contributions:

- Installed Azure Storage Dependencies
- Set Up Project Structure 
- Created the MenuItem model
- Created the TableStorageService
- Created the CreateMenuItem function
- Created the GetAllMenuItems function
- Configured the application to communicate with Azurite Table Storage

## Features Implemented

### MenuItem Model

The MenuItem model is responsible for storing CoffeeNChill menu information in
Azure Table Storage.

The model contains:

- PartitionKey – stores the menu category
- RowKey – stores the unique menu item ID
- Name – stores the menu item name
- Description – stores the menu item description
- Price – stores the menu item price
- IsAvailable – stores the availability status

### Azure Table Storage Service

The TableStorageService handles communication between the CoffeeNChill 
Azure Functions and Azure Table Storage. The TableStorageService connects
to the local Azurite storage emulator and uses the MenuItems table to store
menu information.

### Create Menu Item

The CreateMenuItem function accepts menu item information in JSON format
and stores the new menu item in the MenuItems Azure Table. The CreateMenuItem 
also performs validation before creating the menu item.

### Get All Menu Items

The GetAllMenuItems function retrieves the menu items stored in the MenuItems table
and returns them to the user.

## Local Azurite Setup

I used Azurite to emulate Azure Storage during local development.
I started the Azurite Docker container using the following command:

docker run --name coffeenchill-azurite 
  -p 10000:10000 
  -p 10001:10001 
  -p 10002:10002 
  mcr.microsoft.com/azure-storage/azurite

## Commits

- Installed Azure Storage Dependencies and Set Up Project Structure
- Created MenuItem Model to Store Menu Item Details
- Created TableStorageService to Manage MenuItem CRUD Operations
- Created CreateMenuItem to Validate and Create Menu Item
- Created GetAllMenuItems to Retrieve All The Menu Items


