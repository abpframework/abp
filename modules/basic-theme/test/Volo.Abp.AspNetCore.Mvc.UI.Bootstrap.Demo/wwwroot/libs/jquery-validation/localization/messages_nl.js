(function( factory ) {
	if ( typeof define === "function" && define.amd ) {
		define( ["jquery", "../jquery.validate"], factory );
	} else if (typeof module === "object" && module.exports) {
		module.exports = factory( require( "jquery" ) );
	} else {
		factory( jQuery );
	}
}(function( $ ) {

/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: NL (Dutch; Nederlands, Vlaams)
 */
$.extend( $.validator.messages, {
	required: "Dit is een verplicht veld.",
	remote: "Controleer dit veld.",
	email: "Vul hier een geldig e-mailadres in.",
	url: "Vul hier een geldige URL in.",
	date: "Vul hier een geldige datum in.",
	dateISO: "Vul hier een geldige datum in (ISO-formaat).",
	number: "Vul hier een geldig getal in.",
	digits: "Vul hier alleen getallen in.",
	creditcard: "Vul hier een geldig creditcardnummer in.",
	equalTo: "Vul hier dezelfde waarde in.",
	extension: "Vul hier een waarde in met een geldige extensie.",
	maxlength: $.validator.format( "Vul hier maximaal {0} tekens in." ),
	minlength: $.validator.format( "Vul hier minimaal {0} tekens in." ),
	rangelength: $.validator.format( "Vul hier een waarde in van minimaal {0} en maximaal {1} tekens." ),
	range: $.validator.format( "Vul hier een waarde in van minimaal {0} en maximaal {1}." ),
	max: $.validator.format( "Vul hier een waarde in kleiner dan of gelijk aan {0}." ),
	min: $.validator.format( "Vul hier een waarde in groter dan of gelijk aan {0}." ),
	step: $.validator.format( "Vul hier een veelvoud van {0} in." ),

	// For validations in additional-methods.js
	iban: "Vul hier een geldig IBAN in.",
	dateNL: "Vul hier een geldige datum in.",
	phoneNL: "Vul hier een geldig Nederlands telefoonnummer in.",
	mobileNL: "Vul hier een geldig Nederlands mobiel telefoonnummer in.",
	postalcodeNL: "Vul hier een geldige postcode in.",
	bankaccountNL: "Vul hier een geldig bankrekeningnummer in.",
	giroaccountNL: "Vul hier een geldig gironummer in.",
	bankorgiroaccountNL: "Vul hier een geldig bank- of gironummer in.",

	maxWords: $.validator.format( "Vul hier maximaal {0} woorden in." ),
	minWords: $.validator.format( "Vul hier minimaal {0} woorden in." ),
	rangeWords: $.validator.format( "Vul hier tussen {0} en {1} woorden in." ),
	accept: "Vul hier een waarde in met een geldig MIME-type.",
	alphanumeric: "Vul hier alleen letters, cijfers of underscores in.",
	bic: "Vul hier een geldige BIC-code in.",
	creditcardtypes: "Vul hier een geldig creditcardnummer in.",
	currency: "Vul hier een geldige valuta in.",
	integer: "Vul hier een geheel getal in.",
	ipv4: "Vul hier een geldig IPv4-adres in.",
	ipv6: "Vul hier een geldig IPv6-adres in.",
	lettersonly: "Vul hier alleen letters in.",
	letterswithbasicpunc: "Vul hier alleen letters of leestekens in.",
	netmask: "Vul hier een geldig netmasker in.",
	notEqualTo: "Vul hier een andere waarde in. De waarden mogen niet gelijk zijn.",
	nowhitespace: "Spaties zijn niet toegestaan.",
	pattern: "Ongeldig formaat.",
	require_from_group: $.validator.format( "Vul minimaal {0} van deze velden in." ),
	skip_or_fill_minimum: $.validator.format( "Sla deze velden over of vul er minimaal {0} van in." ),
	strippedminlength: $.validator.format( "Vul hier minimaal {0} tekens in." ),
	time: "Vul hier een geldige tijd in tussen 00:00 en 23:59.",
	time12h: "Vul hier een geldige tijd in (12-uursnotatie).",
	cifES: "Vul hier een geldig CIF-nummer in.",
	cpfBR: "Vul hier een geldig CPF-nummer in.",
	mobileUK: "Vul hier een geldig Brits mobiel nummer in.",
	nieES: "Vul hier een geldig NIE-nummer in.",
	nifES: "Vul hier een geldig NIF-nummer in.",
	nipPL: "Vul hier een geldig NIP-nummer in.",
	phonesUK: "Vul hier een geldig Brits telefoonnummer in.",
	phoneUK: "Vul hier een geldig Brits telefoonnummer in.",
	phoneUS: "Vul hier een geldig Amerikaans telefoonnummer in.",
	postalcodeBR: "Vul hier een geldige Braziliaanse postcode in.",
	postalCodeCA: "Vul hier een geldige Canadese postcode in.",
	postalcodeIT: "Vul hier een geldige Italiaanse postcode in.",
	postcodeUK: "Vul hier een geldige Britse postcode in.",
	stateUS: "Vul hier een geldige Amerikaanse staat in.",
	vinUS: "Het opgegeven voertuigidentificatienummer (VIN) is ongeldig.",
	zipcodeUS: "De opgegeven Amerikaanse postcode is ongeldig.",
	ziprange: "Uw postcode moet binnen het bereik 902xx-xxxx tot 905xx-xxxx liggen."
} );
return $;
}));