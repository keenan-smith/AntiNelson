<?php
  session_start();
?>
<?php
  $path_admins = "Database/admins.database";
  $path_users = "Database/users.database";

  $json_admins = json_decode(file_get_contents($path_admins), true);
  $json_users = json_decode(file_get_contents($path_users), true);

  $session_key = (isset($_SESSION["key"]) ? $_SESSION["key"] : null);
  $username = (isset($_SESSION["username"]) ? $_SESSION["username"] : null);
  $HWID = (isset($_POST["HWID"]) ? $_POST["HWID"] : null);
?>
<?php
  function lock(){
    header("Location: login.php");
    die();
  }

  function back(){
    header("Location: users.php");
    die();
  }

  function home(){
    header("Location: home.php");
    die();
  }
?>
<?php
  if(!$session_key || !$username)
    lock();
  if($json_admins[$username]["session"] != $session_key)
    lock();
  if($json_admins[$username]["lastonline"] == null || (time() - $json_admins[$username]["lastonline"]) >= 600)
    lock();

  $json_admins[$username]["lastonline"] = time();
  file_put_contents($path_admins, json_encode($json_admins));
?>
<?php
  if(isset($_POST["btn_Home"]))
    home();
  if(isset($_POST["btn_Back"]))
    back();
  if(!$HWID)
    back();

  if(isset($_POST["btn_Save"])){
    $json_users[$HWID]["isPremium"] = isset($_POST["chk_Premium"]);
    $json_users[$HWID]["isBlacklisted"] = isset($_POST["chk_Blacklist"]);

    file_put_contents($path_users, json_encode($json_users));
    back();
  }
  if(isset($_POST["btn_Delete"])){
    unset($json_users[$HWID]);

    file_put_contents($path_users, json_encode($json_users));
    back();
  }
?>
<html>
  <head>
    <title>ManPAD User Edit</title>
  </head>
  <body>
    <form action="user.php" method="post">
      <input type="hidden" name="HWID" <?php echo("value='" . $HWID . "'") ?> />
      Steam Name: <?php echo(isset($json_users[$HWID]["steam_name"] ? urldecode($json_users[$HWID]["steam_name"]) : "None"); ?>
      <br /><br />
      Steam 64: <?php echo(isset($json_users[$HWID]["steam_64"] ? urldecode($json_users[$HWID]["steam_64"]) : "None"); ?>
      <br /><br />
      HWID: <?php echo($HWID); ?>
      <br /><br />
      IP: <?php echo($json_users[$HWID]["IP"]); ?>
      <br /><br />
      Last Run: <?php echo(date("H:i:s d.m.Y", $json_users[$HWID]["lastrun"])); ?>
      <br /><br />
      <input type="checkbox" name="chk_Premium" <?php echo(($json_users[$HWID]["isPremium"] ? "checked" : "")) ?> />Is Premium
      <input type="checkbox" name="chk_Blacklist" <?php echo(($json_users[$HWID]["isBlacklisted"] ? "checked" : "")) ?> />Is Blacklisted
      <br /><br />
      <input type="submit" name="btn_Save" value="Save" />
      <input type="submit" name="btn_Delete" value="Delete" />
      <input type="submit" name="btn_Home" value="Go Home" />
      <input type="submit" name="btn_Back" value="Go Back" />
    </form>
  </body>
</html>
