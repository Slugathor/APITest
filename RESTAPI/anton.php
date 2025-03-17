<?php
$servername = "localhost";
$username = "myBackEndApp";
$password = "myBackEndApp";
$database = "backend";

// Create connection
$conn = new mysqli($servername, $username, $password, $database);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$uri = $_SERVER['REQUEST_URI'];
$method = $_SERVER['REQUEST_METHOD'];
switch ($method | $uri) {
    // 1) get a list of all active players
    case ($method == 'GET' && $uri == '/api/players/active'):
        $query = "SELECT A.displayname FROM `player` A, `account` B WHERE A.id = B.player_id AND B.state = 1;";
        $result = $conn->query($query);
        if($result && $result->num_rows){
            $rows = $result->fetch_all(MYSQLI_ASSOC);
            $names=array();
            foreach ($rows as $row) {
                // construct json response of the names
                $names[]=$row["displayname"];
            }
            header('Content-Type: application/json');
            echo json_encode($names);
        }
        break;
    // 2) get a list of characters that belong to blocked/suspended users
   case ($method == 'GET' && $uri == '/api/characters/inactive'):
        // TODO: query database and respond
        echo "get a list of characters that belong to blocked/suspended"; // just a testing echo to see if we're catching the request
        break;
    // 3) Select team members' contact information (e.g. Discord) for a character
   case ($method == 'GET' && preg_match('/\/api\/characters\/contact\/[1-9]/', $uri)):
        $id = basename($uri);
        // constructing a query, placing the $id value from request parameters into WHERE clause of the selection statement:
        $query = "SELECT P.discord FROM `plcharacter` C, `plcharacter` C2, `player` P WHERE C.id = ". $id ." AND C2.team = C.id AND C2.player_id = P.id ;";
        $result = $conn->query($query);  // execute the query in database, retrieving result object
        $rows = $result->fetch_all(MYSQLI_ASSOC); // get an associative array (key=>value) from result 
                                                 // object, consisting of rows and columns eithin them
        $contacts=array();
        if($result && $result->num_rows){   // check if result exists and contained at least one row
            foreach ($rows as $row) {
                $contacts[]=$row["discord"]; // extract value of column "discord" from each row and append to array $contacts
            }
        }
        header('Content-Type: application/json'); // set response headers correctly
        echo json_encode($contacts);    // crate response body, encode array into json serialized text
        break;
    // 4) Get an average XP from all past campaigns of a character
    case(false):
        break;
    // 5) Find the most succeeded campaign for a player
    case(false):
        break;
    // 7) Insert new campaign for a team
    case($method == 'POST' && preg_match('/\/api\/teams\/campaigns\/[1-9]/', $uri)):
        $id = basename($uri); // get the team id from the $uri of request
        /*
        * We could extract all necessary data from the request body, when POST'ed, as follows:
            $requestBody = json_decode(file_get_contents('php://input'), true);
            $world = $requestBody['world']; // getting world_id for the insertion
            $world = $requestBody['mission']; // getting mission_id for the insertion
            $starttime = $requestBody['start']; // getting value for the start datetime
        * and those could replace the static values in the insertion query below:
        **/
        // first create query to insert a new campaign
        $query = "INSERT INTO `campaign` (`world_id`, `mission_id`, `achievement_id`, `starttime`, `endtime`) VALUES (1, 1, 0, '2025-02-12 12:00:00', NULL);";
        if ($conn->query($query) === TRUE) { // check the insertion succeeded
            $last_id = $conn->insert_id;     // get the automatic id of the new row inserted above
            // construct insertion ofr associating a team with the new campaign, using the campaign id of insertoin above
            // and team id from the request:
            $query2 = "INSERT INTO `team_campaign` (`team_id`, `campaign_id`) VALUES (".$id.",".$last_id.");";
            $conn->query($query2); // perform the query too
        }
        header('Content-Type: application/json');
        echo json_encode(true); // return true to let client now we're done (successfully)
        break;
    // 8) Delete account of a player being blocked longer than three months
    case(false):
        break;
    // 9) Get a sum of XP of all the player's characters
    case(false):
        break;
    // 10) Update player's level by one (assume integer from 1 to 100)
    case(false):
        break;
   default:
       http_response_code(404);
       echo json_encode(['error' => "We cannot find what you're looking for."]);
       break;
}
$conn->close();
?>