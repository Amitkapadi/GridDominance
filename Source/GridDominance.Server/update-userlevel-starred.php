<?php

require 'internals/backend.php';


function run() {
	global $pdo;

	$userid        = getParamUIntOrError('userid');
	$password      = getParamSHAOrError('password');
	$appversion    = getParamStrOrError('app_version');
	$levelid       = getParamLongOrError('levelid');
	$starred       = getParamBoolOrError('star');

	$signature     = getParamStrOrError('msgk');

	check_commit_signature($signature, [$userid, $password, $appversion, $levelid, $starred?'true':'false']);

	//----------

	$user = GDUser::QueryOrFail($pdo, $password, $userid);
	$user->UpdateLastOnline($appversion);

	//----------

	$stmt = $pdo->prepare('SELECT starred FROM userlevels_highscores WHERE userid = :uid AND levelid = :lid');
	$stmt->bindValue(':uid', $userid, PDO::PARAM_INT);
	$stmt->bindValue(':lid', $levelid, PDO::PARAM_INT);
	executeOrFail($stmt);
	$currstarred = $stmt->fetchColumn()!=0;


	$stmt = $pdo->prepare('SELECT stars, userid FROM userlevels WHERE id = :lid');
	$stmt->bindValue(':lid', $levelid, PDO::PARAM_INT);
	executeOrFail($stmt);
	$ulcol = $stmt->fetch();

	$allstars = $ulcol['stars'];
	$authorid = $ulcol['userid'];

	if ($currstarred && !$starred)
	{
		// unset

		$stmt = $pdo->prepare('UPDATE userlevels_highscores SET starred=0 WHERE userid = :uid AND levelid = :lid');
		$stmt->bindValue(':uid', $userid, PDO::PARAM_INT);
		$stmt->bindValue(':lid', $levelid, PDO::PARAM_INT);
		executeOrFail($stmt);

		$stmt = $pdo->prepare('UPDATE userlevels SET stars=stars-1 WHERE id = :lid');
		$stmt->bindValue(':lid', $levelid, PDO::PARAM_INT);
		executeOrFail($stmt);
		$allstars -= 1;

		$stmt = $pdo->prepare('UPDATE users SET score_stars=score_stars-1 WHERE userid = :uid');
		$stmt->bindValue(':uid', $authorid, PDO::PARAM_INT);
		executeOrFail($stmt);

		outputResultSuccess([ 'updated' => false, 'value' => false, 'stars' => $allstars, 'user' => $user ]);
	}
	else if (!$currstarred && $starred)
	{
		// set

		$stmt = $pdo->prepare('UPDATE userlevels_highscores SET starred=1 WHERE userid = :uid AND levelid = :lid');
		$stmt->bindValue(':uid', $userid, PDO::PARAM_INT);
		$stmt->bindValue(':lid', $levelid, PDO::PARAM_INT);
		executeOrFail($stmt);

		$stmt = $pdo->prepare('UPDATE userlevels SET stars=stars+1 WHERE id = :lid');
		$stmt->bindValue(':lid', $levelid, PDO::PARAM_INT);
		executeOrFail($stmt);
		$allstars += 1;

		$stmt = $pdo->prepare('UPDATE users SET score_stars=score_stars+1 WHERE userid = :uid');
		$stmt->bindValue(':uid', $authorid, PDO::PARAM_INT);
		executeOrFail($stmt);

		outputResultSuccess([ 'updated' => false, 'value' => true, 'stars' => $allstars, 'user' => $user ]);
	}
	else if ($currstarred && $starred)
	{
		// keep set

		outputResultSuccess([ 'updated' => false, 'value' => true, 'stars' => $allstars, 'user' => $user ]);
	}
	else if (!$currstarred && !$starred)
	{
		// keep unset

		outputResultSuccess([ 'updated' => false, 'value' => false, 'stars' => $allstars, 'user' => $user ]);
	}
	else
	{
		outputError(ERRORS::INTERNAL_EXCEPTION, "WTF else chain: [$currstarred] [$starred]", LogLevel::ERROR);
	}
}



try {
	init("update-userlevel-starred");
	run();
} catch (Exception $e) {
	outputErrorException(Errors::INTERNAL_EXCEPTION, 'InternalError', $e, LOGLEVEL::ERROR);
}