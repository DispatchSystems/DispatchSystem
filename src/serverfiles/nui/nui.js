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
            CloseAllMenus();
        }
    } );
} )
function CloseAllMenus() {
    $( "div" ).each( function( i, obj ) {
        var element = $( this );
        element.hide();
    } );
}

function init() {
    $( ".option" ).each( function( i, obj ) {

        if ( $( this ).attr( "data-action" ) ) {
            $( this ).click( function() { 
                var data = $( this ).data( "action" ); 

                sendData( "ButtonClick", data ); 
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
        if ( datab != "ok" ) {
            console.log( datab );
        }            
    } );
}