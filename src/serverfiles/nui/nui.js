$( function() {
    init();

    var ofcContainer = $( "#leo" );
    var civContainer = $( "#civ" );

    window.addEventListener( 'message', function( event ) {
        var item = event.data;
        
        if ( item.showleomenu ) {
            civContainer.hide()
            ofcContainer.show();
        }
        if ( item.showcivmenu ) {
            ofcContainer.hide();
            civContainer.show();
        }
        if ( item.hidemenus ) {
            closeItems();
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
            $("#" + parent).show();
        }
    } );
}

function closeItems() {
    $( "div" ).each( function( i, obj ) {
        var element = $( this );
        element.hide();
    } );
}

function init() {
    $(".menu").each(function(i,obj) {
        if ( $(this).attr("data-parent")) {
            $(this).append("<button class='option back' onclick='back()'>Back</button>");
        }
        $(this).append("<button class='option x' data-action='common exit test'>Exit</button>");
    });

    $( ".option" ).each( function( i, obj ) {

        if ( $( this ).attr( "data-action" ) ) {
            $( this ).click( function() { 
                var dataArr = $( this ).data( "action" ).split(" ");

                sendData( dataArr[0], dataArr[1] ); 
            } )
        }

        if ( $( this ).attr( "data-sub" ) ) {
            $( this ).click( function() {
                var menu = $( this ).data( "sub" );
                var element = $( "#" + menu ); 
                element.show();
                $( this ).parent().hide();  
            } )
        }
    } );
}

function sendData( name, data ) {
    $.post( "http://dispatchsystem/" + name, JSON.stringify( data ), function( datab ) {
        if ( datab != "OK" ) {
            console.log( datab );
        }            
    } );
}