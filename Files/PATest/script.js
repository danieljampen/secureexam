/**
 * Created by jampe on 23.09.2014.
 * This is just a "doable test"
 */

// Global Variables
var PBKDF2ITERATIONS = 1;						// amount of PBKDF2 Iterations


/*
 *	FUNCTION encrypt()
 *	arguments: none
 *
 *  description: encrypt the questions. this task will be done by our desktop application.
 */
function encrypt() {
	/*
	 *
	 * 	THIS WOULD BE DONE BY OUR DESKTOP APPLICATION
	 * 	encrypts questions with masterkey and encrypts the masterkey for each user (each using AES)
	 *
	 * */
	// Students array: add more students here, rest is dynamic
	var students = [
		["jampedan","S12198320"],
		["lukessim","S12198347"],
		["tothpat1","S12198350"],
		["goncadan","S12198368"]
	];

	// initialize Variables
	var userCredentials = [];
	var encryptedMasterKeys = [];
	var pbkdf2Salts = [];
	var questions = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";

	// Encrypt Questions with AES (128 Bit Key and IV used)
	var AESkey = CryptoJS.enc.Hex.stringify(CryptoJS.lib.WordArray.random(256/8));		// convert from WordArray to String as Key typeOf string
	var encryptedQuestions = CryptoJS.AES.encrypt( questions, AESkey);


	// loop through users
	for( i = 0; i < students.length; i++) {
		// encrypt AES key for each user with given PW plus 10 random chars
		userCredentials.push(students[i][0] + students[i][1] + CryptoJS.enc.Hex.stringify(CryptoJS.lib.WordArray.random(5)));

		// generate PBKDF2 salt for this user
		pbkdf2Salts[i] = CryptoJS.enc.Hex.stringify(CryptoJS.lib.WordArray.random(128/8));

		// generate PBKDF2 key
		var key = CryptoJS.enc.Hex.stringify(CryptoJS.PBKDF2(userCredentials[i], pbkdf2Salts[i], { keySize: 256/32 , iterations: PBKDF2ITERATIONS}));

		// actual encryption of the master key
		// ---> We could add PBKDF2 support here (generate key for masterkey encryption)
		var encryted = CryptoJS.AES.encrypt( AESkey, key);
		var key = CryptoJS.enc.Hex.stringify( encryted.key);
		encryptedMasterKeys.push(encryted.toString());

	}


	// print document
	for( i = 0; i < userCredentials.length; i++) {
		document.getElementById("userCredentials").innerHTML += userCredentials[i] + "<br>";
		document.getElementById("userKeyDB").innerHTML += students[i][0] + "," + encryptedMasterKeys[i] + "," + pbkdf2Salts[i] + "<br>";
	}
	document.getElementById("secretStuff").innerHTML = encryptedQuestions.toString();
}


/*
 *	FUNCTION decrypt()
 *	arguments: - (string) divUserKeyDB = div ID where the UserKey Information is written
 *			   - (string) divSecretStuff = div ID where the encrypted data is stored
 *			   - (string) divQuestions = div ID where the decrypted data will be written into
 *
 *  description: decrypt the questions.
 */
function decrypt(divUserKeyDB, divSecretStuff, divQuestions) {
	// load userKeyDB
	var userKeyDB = document.getElementById(divUserKeyDB).innerHTML.split("<br>");
	// remove empty last entry from userKeyDB
	userKeyDB.pop();
	// split username from secret
	for( i = 0; i < userKeyDB.length; i++) {
		userKeyDB[i] = userKeyDB[i].split(",");
	}

	// Ask for Secret
	var userSecret = prompt("UserCredentials:");

	// start time measurement
	var startTime = new Date();

	// extract Username for comparison with userKeyDB
	var userName = userSecret.substring(0,8);

	// get encryptedMasterkey cypher for this user
	var masterkeyCypher;
	var salt;
	for( i = 0; i < userKeyDB.length; i++ ) {
		if( userName == userKeyDB[i][0]){
			masterkeyCypher = userKeyDB[i][1];
			salt = userKeyDB[i][2];
			break;
		}

	}

	// generate PBKDF2 key
	var key = CryptoJS.enc.Hex.stringify(CryptoJS.PBKDF2(userSecret, salt, { keySize: 256/32 , iterations: PBKDF2ITERATIONS}));

	// decrypt masterkey
	var masterKey = CryptoJS.AES.decrypt(masterkeyCypher , key).toString(CryptoJS.enc.Utf8);

	// decrypt questions
	var decrypted = CryptoJS.AES.decrypt(document.getElementById(divSecretStuff).innerHTML, masterKey);

	// measure timediff
	document.getElementById("decryptFieldsetLegend").innerHTML = "Erscheint auf Bildschirm | decrypted by: " + userName + " | decryption time: " + (new Date() - startTime) + "ms";

	// print questions
	document.getElementById(divQuestions).innerHTML = decrypted.toString(CryptoJS.enc.Utf8);

}