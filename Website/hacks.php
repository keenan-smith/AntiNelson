<?php
  session_start();
?>
<?php
  $path_admins = "Database/admins.database";
  $path_hacks = "Database/hacks.database";

  $json_admins = json_decode(file_get_contents($path_admins), true);
  $json_hacks = json_decode(file_get_contents($path_hacks), true);

  $session_key = (isset($_SESSION["key"]) ? $_SESSION["key"] : null);
  $username = (isset($_SESSION["username"]) ? $_SESSION["username"] : null);
?>
<?php
  function lock(){
    header("Location: login.php");
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
?>
<?php
  if(isset($_POST["btn_Save"])){
    $json_hacks["freeBypass"] = isset($_POST["chk_freeBypass"]);
    $json_hacks["freeHack"] = isset($_POST["chk_freePremium"]);
    $json_hacks["running"] = isset($_POST["chk_running"]);
    $json_hacks["unturnedVersion"] = $_POST["txt_version"];

    file_put_contents($path_hacks, json_encode($json_hacks));
  }
  if(isset($_POST["btn_Home"]))
    home();
?>
<html>
  <head>
    <title>ManPAD Manager</title>
  </head>
  <body>
    <form action="hacks.php" method="post">
      <input type="checkbox" name="chk_freeBypass" <?php echo(($json_hacks["freeBypass"] ? "checked" : "")); ?> />Free BE Bypass
      <br /><br />
      <input type="checkbox" name="chk_freePremium" <?php echo(($json_hacks["freeHack"] ? "checked" : "")); ?> />Free Premium Hack
      <br /><br />
      <input type="checkbox" name="chk_running" <?php echo(($json_hacks["running"] ? "checked" : "")); ?> />Hack Running
      <br /><br />
      Unturned Version: <input type="text" name="txt_version" <?php echo("value='" . $json_hacks["unturnedVersion"] . "'"); ?> />
      <br /><br />
      <input type="submit" name="btn_Save" value="Save" />
      <input type="submit" name="btn_Home" value="Go Home" />
    </form>
  </body>
</html>
