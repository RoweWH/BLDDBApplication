# BLD DB API
This is an API that connects to a database of algorithms used for Rubik's Cube blindfolded solving.

## Current Functionality
* GET - returns a list of algorithms for a specific case
* POST (2 Methods)
  1.  PostAlgorithmByCase - Validates that an algorithm matches a specific case, and then adds to the database. Meant for future CRUD applications.
  2.  PostAlgorithms - For importing large sets of algorithms in a file at once. This method determines which algorithms are valid, then organizes them into the database by case. Meant for future database admin application.
     
* DELETE - deletes algorithm

## Technologies Used
* C#
* .NET8
* SQL

## TODO
* Parity algorithms are a must
* Other advanced algsets maybe, not sure yet
  
