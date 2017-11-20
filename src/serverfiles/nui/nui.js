var lastmenu;

$( function() {
    init();

    var ofcContainer = $( "#leo" );
    var civContainer = $( "#civ" );

    window.addEventListener( 'message', function( event ) {
        var item = event.data;
        
        if ( item.showleomenu ) {
            civContainer.hide();
            ofcContainer.show();
            lastmenu = ofcContainer;
        }
        if ( item.showcivmenu ) {
            ofcContainer.hide();
            civContainer.show();
            lastmenu = ofcContainer;
        }
        if (item.openlastmenu) {
            lastmenu.show();
        }
        if ( item.hidemenus ) {
            $("div").hide();
        }
    } );
} )

function back() {
    $( "div" ).each( function( i, obj ) {
        if ( !$(this).is(":visible") ) {
            return;
        }
        
        if ($(this).attr("data-parent")) {
            var parent = $(this).data("parent");

            $(this).hide();
            var parentMenu = $("#" + parent);
            parentMenu.show();
            lastmenu = parentMenu;
        }
    } );
}

function exit() {
    $("div").each(function(i,obj) {
        if (!$(this).is(":visible")) {
            return;
        }

        $(this).hide();
        
        send("common", ["exit"]);
    });
}

function init() {
    $(".menu").each(function(i,obj) {
        if ( $(this).attr("data-parent")) {
            $(this).append("<button class='option back' onclick='back()'>Back</button>");
        }
        $(this).append("<button class='option x' onclick='exit()'>Exit</button>");
    });

    $( ".option" ).each( function( i, obj ) {

        if ( $( this ).attr( "data-action" ) ) {
            $( this ).click( function() { 
                var dataArr = $( this ).data( "action" ).split(" ");
                var data = [];
                
                for (var i = 1; i < dataArr.length; i++) {
                    data[i - 1] = dataArr[i];
                }

                send( dataArr[0], data ); 
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
    $.post( "http://dispatchsystem/" + name, JSON.stringify(data), function( datab ) {
        if ( datab != "OK" ) {
            console.log( datab );
        }            
    } );
}