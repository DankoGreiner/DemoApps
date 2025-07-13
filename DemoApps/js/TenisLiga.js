function testTL()
{
    alert('TestTL');
}

function SetSezona(sezonaId, sezonaNaziv) {
    $("#hidOdabranaSezonaId").val(sezonaId);
    //$("#hidOdabranaSezonaNaziv").val(sezonaNaziv);
    $("#btnOdabranaSezona").text(sezonaNaziv);
    $("#btnOdabranaSezona").append(' <span class="caret"></span>');
    $("#btnLoadTablica").click();
}

function SetLiga(ligaId) {
    $(".ddlLiga").val(ligaId);
    $("#hidOdabranaLigaId").val(ligaId);
    $('#btnOdabranaLiga').click();
}

function setSelectedIgracRow()
{
    if ($("#hidSelectedIgracId").val() != "")
        $("#trIgracId" + $("#hidSelectedIgracId").val()).addClass('text-bold');
}

function pageLoad() {
    //alert('load'); // uvijek se izvršava

    setSelectedIgracRow();

    //Chosen
    $('.chosen').chosen();
    $('.chosenTL').chosen();

    //Datepicker
    $.datepicker.setDefaults(
      $.extend(
        { 'dateFormat': 'dd-mm-yy' },
        $.datepicker.regional['hr']
      )
    );
    $('.datepicker').datepicker({ dateFormat: 'd.m.yy', firstDay: 1 });

    //ddl Sezona / liga
    $("#btnTest").val('pageLoadContent');

    $('.ddlLiga').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        $("#hidOdabranaLigaId").val(valueSelected);
        $('#btnOdabranaLiga').click();
    });

    $('.ddlSezona').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        $("#hidOdabranaSezonaId").val(valueSelected);
        $('#btnOdabranaSezona').click();
    });


    //Clickable igrač
    $('.clickableIgrac').click(function () {
        var igracId = $(this).attr('igracId');
        location.href = window.location.origin + "/PlayerInfo.aspx?IgracId=" + igracId;
    });

    //Bolda pobjednika
    $('.IsPobjednikTrue').addClass('text-bold')

    pageLoadContent();

}
function setSezonaButton() {
    if ($("#hidOdabranaSezonaNaziv").val() != "") {
        $("#btnOdabranaSezona").text($("#hidOdabranaSezonaNaziv").val());
        $("#btnOdabranaSezona").append(' <span class="caret"></span>');
    }
}
function setLigaButton() {
    if ($("#hidOdabranaLigaNaziv").val() != "") {
        $("#btnOdabranaLiga").text($("#hidOdabranaLigaNaziv").val());
        $("#btnOdabranaLiga").append(' <span class="caret"></span>');
    }
}

function showLiga(sezonaId, ligaId) {
    var currentPage = window.location.pathname.substring(1, window.location.pathname.length).toLowerCase();
    //alert(currentPage);
    if (currentPage == "default.aspx" || currentPage == "")
    {
        SetLiga(ligaId);
        $('html, body').animate({
            scrollTop: 0
        }, 500);
    }
    else
    {
        window.location = "Tablica.aspx?SezonaId=" + sezonaId + "&LigaId=" + ligaId;
    }
}


function showTLDiv(divToShow) {
    $("#leftCol").addClass("hidden-xs");
    $("#TLContent").addClass("hidden-xs");
    $("#rightCol").addClass("hidden-xs");

    if (divToShow == "TLTalbica" || divToShow == "TLZadnjiRezultati") {
        $("#leftCol").removeClass("hidden-xs");
        $("#TLTalbica").hide();
        $("#TLZadnjiRezultati").hide();
        $("#" + divToShow).show();
    }
    else if (divToShow == "TLForum" || divToShow == "TLKalendar") {
        $("#rightCol").removeClass("hidden-xs");
        $("#TLForum").hide();
        $("#TLKalendar").hide();
    }
    else {
        $("#TLContent").removeClass("hidden-xs");
    }
    $("#" + divToShow).show();

}


$('#ddlTipNatjecanja').on('change', function (e) {
    var optionSelected = $("option:selected", this);
    var valueSelected = this.value;
    $("#hidOdabranaLigaId").val(valueSelected);
    $('#btnOdabranaLiga').click();
});

function redirectToNajava(dogadajId) {
    window.location = "Najava.aspx?DogadajId=" + dogadajId;
}

function masterPageLoad()
{
    //alert('enter');

    setActiveLink();

    $("#txtTrazi").on('keyup', function (e) {
        if (e.keyCode == 13) {
            traziIgrac();
        }
    });
}

function setActiveLink()
{
    //Set active item in left menu
    var acitiveSideLink = window.location.pathname.substring(1, window.location.pathname.length).toLowerCase();
    $('a[href=\'' + acitiveSideLink + '\']').closest("li").addClass("active");
}

function goToTablica(sezonaId, ligaId)
{
    window.location = "Tablica.aspx?SezonaId=" + sezonaId + "&LigaId=" + ligaId;
}

function getParameterByName(name, url) {
    if (!url) {
        url = window.location.href;
    }
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function traziIgrac()
{
    var trazi = $("#txtTrazi").val();
    window.location = "Trazi.aspx?trazi=" + trazi;
}

function imgError(image) {
    image.onerror = "";
    image.src = "/images/players/logo.png";

    return true;
}