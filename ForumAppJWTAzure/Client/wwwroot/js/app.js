var resize;
var base64data;

window.Crop = {
    image: function (component) {

        setTimeout(() => {

            var cor = document.getElementById('upload-demo');
            resize = new Croppie(cor, {
                enableExif: true,
                viewport: {
                    width: 150,
                    height: 150,
                    type: 'circle'
                },
                boundary: {
                    width: 300,
                    height: 300
                }
            });

            var input = document.getElementById('upload').files[0];
            if (input) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    //document.getElementsByClassName('upload-demo').classList.add('ready');
                    resize.bind({
                        url: e.target.result
                    });
                }
                reader.readAsDataURL(input);
            }
            else {
                alert("Sorry - you're browser doesn't support the FileReader API");
            }
        }, 150);

    },
    responses: function (component) {
        resize.result('blob').then(function (blob) {
            var reader = new FileReader();
            reader.readAsDataURL(blob);
            reader.onloadend = async function () {
                base64data = reader.result;
                var i = document.getElementById('result');
                var img = document.createElement('img');
                img.src = base64data;
                i.appendChild(img);
                return component.invokeMethodAsync('ResponseMethod', base64data);
            }
        });

    }
};

// Waits for the ModalPage animation to complete when the page is closed. Once complete, it calls ModalClosedAsync method to actually close the page.
export function waitforannimation(dotNetHelper) {
    var modalWrapper = document.getElementById('modal-container-scrollable');

    /*For when object has fully faded*/
    modalWrapper.addEventListener("animationend", function () {
        if (this.className == "modal-container hide") {
            this.style.display = "none";
            return dotNetHelper.invokeMethodAsync('ModalClosedAsync')

        }
    }.bind(modalWrapper));

}

export function WaitForProfilePictureUploadButton() {
    document.querySelector('#upload').click();
}

export function SetDotNetHelper(dotNetHelper) {
    window.dotNetHelper = dotNetHelper;
}

export function addCss(fileName) {

    var head = document.head;
    var link = document.createElement("link");

    link.type = "text/css";
    link.rel = "stylesheet";
    link.href = fileName;
    link.className = "theme";

    head.appendChild(link);
}

export function removeThemes() {

    var elements = document.getElementsByClassName("theme");

    if (elements.length == 0) return;

    for (var i = 0, l = elements.length; i < l; i++) {
        elements[i].remove();
    }    
}

export function ScrollToBottom(id) {
    var objDiv = document.getElementById(id);
    setTimeout(() => {        
        objDiv.scrollTo({ top: objDiv.scrollHeight, behavior: "smooth" });
    }, 0)

    var button = document.getElementById("ScrollToBottom_button");
    

    objDiv.addEventListener('scroll', function (e) {        
        if (objDiv.scrollTop >= objDiv.clientHeight) {

            if (button.classList.contains('fading') == false) {
                console.log('Fading');
                button.classList.remove('showing');
                button.classList.add('fading');
            }
        }
        else {            

            if (button.classList.contains('showing') == false) {
                console.log('Showing');
                button.classList.remove('fading');
                button.classList.add('showing');
            }            
        }
    });
}

export function highlightSnippet() {
    document.querySelectorAll('pre code').forEach((el) => {
        hljs.highlightBlock(el);
    });
}



    
export function setModalDraggableAndResizable()
{
    $('.modal-content').resizable({
        //alsoResize: ".modal-dialog",
        minHeight: 300,
        minWidth: 300
    });      

      $('.modal-dialog').draggable();

      $('.modal-dialog').resize(function () {
          // do something when the element is resized
          $('.modal-content').css({
              width: parent.width,
              height: parent.height
          });
      });
}