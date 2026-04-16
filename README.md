# BLD DB API
This is an API that connects to a database of algorithms used for Rubik's Cube blindfolded solving. There are currently 3 types of algorithms it stores:
* Edge Commutators
* Corner Commutators
* Parity Algorithms (Standard 2e2c, LTCT and 2TC)

## Current Functionality
* GET - returns a list of algorithms for a specific case
* POST (2 Methods)
  1.  PostAlgorithmByCase - Validates that an algorithm matches a specific case, and then adds to the database.
  2.  PostAlgorithms - For importing large batches of algorithms at once. This method determines which algorithms are valid, then organizes them into the database by case.
     
* DELETE - deletes algorithm

## Technologies Used
* C#
* .NET8
* SQL

## TODO
* expand get requests
* Other advanced algsets

  
