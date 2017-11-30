var lastmenu;
var resourcename;

$( function() {
    init();

    var ofcContainer = $( "#leo" );
    var civContainer = $( "#civ" );

    window.addEventListener( 'message', function( event ) {
        var item = event.data;
        
        if ( item.showleomenu ) {
            $("div").hide();
            ofcContainer.show();
            $("#ofcinfo").show();
            lastmenu = ofcContainer;
        }
        if ( item.showcivmenu ) {
            $("div").hide();
            civContainer.show();
            $("#civinfo").show();
            lastmenu = ofcContainer;
        }
        if (item.openlastmenu) {
            lastmenu.show();
        }
        if ( item.hidemenus ) {
            lastmenu.hide();
        }
        if ( item.setname ) {
            resourcename = item.metadata;
        }
        if ( item.pushback ) {
            // getting info arr
            var civinfo = item.data[0];
            var ofcinfo = item.data[1];

            // getting civ elements
            var civname = $("#civname");
            var civwarrant = $("#civwarrant");
            var civcitations = $("#civcit");
            // getting veh elements
            var vehplate = $("#vehplate");
            var vehstolen = $("#vehstolen");
            var vehregi = $("#vehregi");
            var vehinsured = $("#vehinsured");
            // getting ofc elements
            var ofcsign = $("#ofcsign");
            var ofcstatus = $("#ofcstatus");
            var ofcass = $("#ofcass");
            
            // setting civ stuff
            civname.text(civinfo[1] + ", " + civinfo[0]);
            civwarrant.text(civinfo[3]);
            civcitations.text(civinfo[2]);
            // setting veh stuff
            vehplate.text(civinfo[4]);
            vehstolen.text(civinfo[5]);
            vehregi.text(civinfo[6]);
            vehinsured.text(civinfo[7]);
            // setting ofc stuff
            ofcsign.text(ofcinfo[0]);
            ofcstatus.text(ofcinfo[1]);
            ofcass.text(ofcinfo[2]);
        }
    } );
} )

function back(sender) {
    var item = $(sender).parent();

    var parent = item.data("parent");

    item.hide();
    var parentMenu = $("#" + parent);
    parentMenu.show();
    lastmenu = parentMenu;
}

function exit() {
    $("div").hide();

    send("common", ['exit'])
}

function arrSkip(arr, count) {
    var data = [];

    for (var i = count; i < arr.length; i++) {
        data[i - count] = arr[i];
    }

    return data;
}

function init() {
    $(".menu").each(function(i,obj) {
        if ( $(this).attr("data-parent")) {
            $(this).append("<button class='option back' onclick='back(this)'>Back</button>");
        }
        $(this).append("<button class='option x' onclick='exit()'>Exit</button>");
    });

    $( ".option" ).each( function( i, obj ) {

        if ( $( this ).attr( "data-action" ) ) {
            $( this ).click( function() { 
                var dataArr = $( this ).data( "action" ).split(" ");

                send( dataArr[0], arrSkip(dataArr, 1) ); 
            } )
        }

        if ( $( this ).attr( "data-sub" ) ) {
            $(this).addClass("sub");
            $( this ).click( function() {
                var menu = $( this ).data( "sub" );
                var element = $( "#" + menu ); 
                element.show();
                lastmenu = element;
                $( this ).parent().hide();  
            } )
        }
    } );
}

function send( name, data ) {
    $.post( "http://" + resourcename + "/" + name, JSON.stringify(data), function( datab ) {
        if ( datab != "OK" ) {
            console.log( datab );
        }
    } );
}