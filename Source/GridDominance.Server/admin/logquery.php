<?php

require_once '../internals/backend.php';
require_once '../internals/utils.php';

init("admin");

foreach (listLogFiles() as $log) {
	if ($log['name'] == $_GET['id']) { echo nl2br($log['content']); return; }
}

echo 'LOG FILE NOT FOUND';
