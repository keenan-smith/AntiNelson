<?php
  // Error -2 -> Offline
  // Error -1 -> Blocked/Blacklisted
  // Error 10 -> Not enough POST data sent to server
  // Error 20 -> HWID doesn't exist
  // Error 21 -> Steam64 doesn't exist
  // Error 22 -> Fake steam profile
  // Error 23 -> Invalid unturned version

  header('Content-Type: application/json');
?>
<?php
  $json_return = array(
    "success" => null,
    "data" => null
  );
  $json_return_error = array(
    "error_ID" => null,
    "error_message" => null
  );

  $path_users = "Database/users.database";
  $path_hacks = "Database/hacks.database";
  $path_1_bypasser = "Files/1-bypasser.txt";
  $path_1_injector = "Files/1-injector.txt";
  $path_1_loader = "Files/1-loader.txt";
  $path_1_executor = "Files/1-executor.txt";
  $path_2_free = "Files/2-Free.txt";
  $path_2_premium = "Files/2-Premium.txt";

  $json_users = json_decode(file_get_contents($path_users), true);
  $json_hacks = json_decode(file_get_contents($path_hacks), true);

  $stage = (isset($_POST["stage"]) ? $_POST["stage"] : null);
  $unturned_version = (isset($_POST["version"]) ? $_POST["version"] : null);

  $IP = $_SERVER["REMOTE_ADDR"];
  $steam_name = (isset($_POST["steam_name"]) ? $_POST["steam_name"] : null);
  $steam_64 = (isset($_POST["steam_64"]) ? $_POST["steam_64"] : null);
  $HWID = (isset($_POST["HWID"]) ? $_POST["HWID"] : null);
  $lastrun = time();
  $isBlacklisted = false;
  $isPremium = false;
?>
<?php
  function sendError($_errorID, $_errorMessage){
    $json_return_error["error_ID"] = $_errorID;
    $json_return_error["error_message"] = $_errorMessage;

    $json_return["success"] = false;
    $json_return["data"] = $json_return_error;

    die(json_encode($json_return));
  }

  function sendData($_data){
    $json_return["success"] = true;
    $json_return["data"] = $_data;

    die(json_encode($json_return));
  }
?>
<?php
  if(!$stage || !$HWID)
    sendError(10, "Not enough arguments!");
  if(!$json_hacks["running"])
    sendError(-2, "Hack is offline!");

  foreach($json_users as $_key => $_data)
    if($_key == $HWID || $_data["IP"] == $IP || (isset($steam_64) ? $_data["steam_64"] == $steam_64 : false))
      if($_data["isBlacklisted"])
        sendError(-1, "Error while parsing your POST data!");

  if($stage == 1){
    if(array_key_exists($HWID, $json_users)){
      $json_users[$HWID]["lastrun"] = $lastrun;
    }
    else{
      $json_users[$HWID] = array(
        "IP" => $IP,
        "steam_name" => null,
        "steam_64" => null,
        "isPremium" => false,
        "isBlacklisted" => false,
        "lastrun" => $lastrun
      );
    }

    file_put_contents($path_users, json_encode($json_users));

    $_returnData = array(
      "loader" => file_get_contents($path_1_loader),
      "executor" => file_get_contents($path_1_executor),
      "injection" => null
    );

    if($json_users[$HWID]["isPremium"] || $json_hacks["freeBypass"])
      $_returnData["injection"] = file_get_contents($path_1_bypasser);
    else
      $_returnData["injection"] = file_get_contents($path_1_injector);

    sendData($_returnData);
  }
  elseif ($stage == 2){
    if(!$steam_name || !$steam_64 || !$unturned_version)
      sendError(10, "Not enough arguments!");
    if(!array_key_exists($HWID, $json_users))
      sendError(20, "Invalid HWID!");
    if($unturned_version != $json_hacks["unturnedVersion"])
      sendError(23, "Outdated hack!");

    $_check = "http://steamcommunity.com/profiles/" . $steam_64 . "/?xml=1";
    $_check_xml = simplexml_load_file($_check);

    if(isset($_check_xml->error))
      sendError(21, "Could not connect to steam network!");
    if(str_replace(" ]]>", "", str_replace("<![CDATA[ ", "", $_check_xml->steamID)) != $steam_name)
      sendError(22, "Invalid data!");
    if(!$json_users[$HWID]["steam_64"] || !$json_users[$HWID]["steam_name"]){
      $json_users[$HWID]["steam_64"] = $steam_64;
      $json_users[$HWID]["steam_name"] = urlencode($steam_name);
      file_put_contents($path_users, json_encode($json_users));
    }

    $_returnData = array(
      "hack" => null
    );

    if($json_users[$HWID]["isPremium"] || $json_hacks["freeHack"])
      $_returnData["hack"] = file_get_contents($path_2_premium);
    else
      $_returnData["hack"] = file_get_contents($path_2_free);

    sendData($_returnData);
  }
?>
