<?php
    /*
        *REALLY* bad at PHP so ignore all of my bad habits or whatever.

        Feel free to fork and edit this so it's better
    */
    class response {
        public $message;
        public $code;

        public function __construct($p1, $p2) {
            $this->message = $p1;
            $this->code = $p2;
        }
    }

    if ($_SERVER['REQUEST_METHOD'] === 'POST') {
        try {
            $ip = $_SERVER['REMOTE_ADDR'];
            $info = filter_input(INPUT_POST, 'dump_json');
            if (!isset($info)) {
                echo json_encode(new response("invalid_argument", 1));
                return;
            }
    
            $index = 0;
            $file = "dumps/".$ip."-".$index.".json";
            while (file_exists($file)) {
                $index = $index + 1;
                $file = "dumps/".$ip."-".$index.".json";
            }
    
            file_put_contents($file, $info);
    
            echo json_encode(new response("success", 0));
        }
        catch (Exception $e) {
            echo $e;
        }
    
        die();
    }
?>

<html>
    <head>
        <title>DispatchSystem Dumping Grounds</title>
        <style>
            body {
                width: 40%;
                margin: 0 auto;
                font-family: Tahoma, Helvetica, sans-serif;
                margin-top: 25px;
            }
        </style>
    </head>
    <body>
        <h1>DispatchSystem Dumping Grounds</h1>
        <p>
            Sorry, but this is only meant for DispatchSystem.
            <br>
            Feel free to press the <- button to go back.
        </p>
    </body>
</html>