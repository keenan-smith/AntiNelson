<?php
  session_start();
?>
<?php
  $path_admins = "Database/admins.database";
  $path_hacks = "Database/hacks.database";
  $path_1_bypasser = "Files/1-bypasser.txt";
  $path_1_executor = "Files/1-executor.txt";
  $path_1_injector = "Files/1-injector.txt";
  $path_1_loader = "Files/1-loader.txt";
  $path_2_free = "Files/2-Free.txt";
  $path_2_premium = "Files/2-Premium.txt";
  $path_3_assetbundle_1 = "Files/3-AssetBundle-1.txt";
  $path_3_assetbundle_2 = "Files/3-AssetBundle-2.txt";

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
  file_put_contents($path_admins, json_encode($json_admins));
?>
<?php
  if(isset($_POST["btn_Save"])){
    if(!empty($_FILES["file_bypasser"]["name"])){
      if(file_put_contents($path_1_bypasser, base64_encode(file_get_contents($_FILES["file_bypasser"]["tmp_name"])))){
        echo("Bypasser uploaded!<br /><br />");
      }else{
        echo("Failed to upload Bypasser<br /><br />");
      }
    }
    if(!empty($_FILES["file_executor"]["name"])){
      if(file_put_contents($path_1_executor, base64_encode(file_get_contents($_FILES["file_executor"]["tmp_name"])))){
        echo("Executor uploaded!<br /><br />");
      }else{
        echo("Failed to upload Executor<br /><br />");
      }
    }
    if(!empty($_FILES["file_injector"]["name"])){
      if(file_put_contents($path_1_injector, base64_encode(file_get_contents($_FILES["file_injector"]["tmp_name"])))){
        echo("Injector uploaded!<br /><br />");
      }else{
        echo("Failed to upload Injector<br /><br />");
      }
    }
    if(!empty($_FILES["file_loader"]["name"])){
      if(file_put_contents($path_1_loader, base64_encode(file_get_contents($_FILES["file_loader"]["tmp_name"])))){
        echo("Loader uploaded!<br /><br />");
      }else{
        echo("Failed to upload Loader<br /><br />");
      }
    }
    if(!empty($_FILES["file_hackfree"]["name"])){
      if(file_put_contents($path_2_free, base64_encode(file_get_contents($_FILES["file_hackfree"]["tmp_name"])))){
        echo("Free Hack uploaded!<br /><br />");
      }else{
        echo("Failed to upload Free Hack<br /><br />");
      }
    }
    if(!empty($_FILES["file_hackpremium"]["name"])){
      if(file_put_contents($path_2_premium, base64_encode(file_get_contents($_FILES["file_hackpremium"]["tmp_name"])))){
        echo("Premium Hack uploaded!<br /><br />");
      }else{
        echo("Failed to upload Premium Hack<br /><br />");
      }
    }
    if(!empty($_FILES["file_assetbundle1"]["name"])){
      if(file_put_contents($path_3_assetbundle_1, base64_encode(file_get_contents($_FILES["file_assetbundle1"]["tmp_name"])))){
        echo("Asset Bundle 1 uploaded!<br /><br />");
      }else{
        echo("Failed to upload Asset Bundle 1<br /><br />");
      }
    }
    if(!empty($_FILES["file_assetbundle2"]["name"])){
      if(file_put_contents($path_3_assetbundle_2, base64_encode(file_get_contents($_FILES["file_assetbundle2"]["tmp_name"])))){
        echo("Asset Bundle 2 uploaded!<br /><br />");
      }else{
        echo("Failed to upload Asset Bundle 1<br /><br />");
      }
    }

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
    <form action="hacks.php" method="post" enctype="multipart/form-data">
      <input type="checkbox" name="chk_freeBypass" <?php echo(($json_hacks["freeBypass"] ? "checked" : "")); ?> />Free BE Bypass
      <br /><br />
      <input type="checkbox" name="chk_freePremium" <?php echo(($json_hacks["freeHack"] ? "checked" : "")); ?> />Free Premium Hack
      <br /><br />
      <input type="checkbox" name="chk_running" <?php echo(($json_hacks["running"] ? "checked" : "")); ?> />Hack Running
      <br /><br />
      Unturned Version: <input type="text" name="txt_version" <?php echo("value='" . $json_hacks["unturnedVersion"] . "'"); ?> />
      <br /><br />
      Uploads:
      <br /><br />
      Bypasser: <input type="file" id="file_bypasser" name="file_bypasser" />
      <br /><br />
      Executor: <input type="file" id="file_executor" name="file_executor" />
      <br /><br />
      Injector: <input type="file" id="file_injector" name="file_injector" />
      <br /><br />
      Loader: <input type="file" id="file_loader" name="file_loader" />
      <br /><br />
      Free Hack: <input type="file" id="file_hackfree" name="file_hackfree" />
      <br /><br />
      Premium Hack: <input type="file" id="file_hackpremium" name="file_hackpremium" />
      <br /><br />
      Asset Bundle 1: <input type="file" id="file_assetbundle1" name="file_assetbundle1" />
      <br /><br />
      Asset Bundle 2: <input type="file" id="file_assetbundle2" name="file_assetbundle2" />
      <br /><br />
      <input type="submit" name="btn_Save" value="Save" />
      <input type="submit" name="btn_Home" value="Go Home" />
    </form>
  </body>
</html>
