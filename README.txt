Overview
 This project is intended to track matches (or games) played for any arbitrary sport but is incomplete.

This project contains the following components:

 Framework level
  SharpEcho.CodeChallenge.Api - This is an implementation of a generic REST API that maps HTTP actions to CRUD operations
  SharpEcho.CodeChallenge.Data - This implements a generic repository and implements CRUD operations on tables following the convention that there is a primary key called Id

 Application level
  SharpEcho.CodeChallenge.Api.Team - This provides functionality to create, update, delete, and find a team note that the project is configured to use Swagger for API browsing
  SharpEchoCodeChallenge - This is the database solution.  The only table that is currently in place is the Team table.

Please implement the following:
 1. Functionality to record a match between two teams.
   a. The result of a match will need to be tracked.  For instance, the Dallas Cowboys played the Atlanta Falcons on 9/20 and the Cowboys won (it is uncessary to track the score - assume no ties are allowed).
 2. The capability to get the overall win-loss for matches between two teams.  For instance, the Cowboys have Falcons have played each other 28 times and the Cowboys have won 17 times.
 4. Create unit tests for any of the above features, add units tests to cover functionality that is that provided automatically at the framework level (see existing Api.Team.Tests which only impements tests for which actual application code was written).
 5. Create a client (can be any client - could be a console app or web app, etc. - that hits the API that does the following:
  a. Adds two teams: Dallas Cowboys and Atlanta Falcons 
  b. Records 28 matches between the 2 teams, with the Cowboys winning 17 of the matches
  c. Returns the overall win-loss for games between the Cowboys and Falcons
