function readURL(input) {
  if (input.files && input.files[0]) {
    var reader = new FileReader();

    reader.onload = function (e) {
      $(".image-upload-wrap").hide();
      $(".file-upload-image").attr("src", e.target.result);
      $(".file-upload-content").show();
      $(".image-title").html(input.files[0].name);
    };

    reader.readAsDataURL(input.files[0]);
  } else {
    removeUpload();
  }
}

function removeUpload() {
  $(".file-upload-input").replaceWith($(".file-upload-input").clone());
  $(".file-upload-content").hide();
  $(".image-upload-wrap").show();
  $(".file-upload-image").attr("src", "#");
  $(".image-title").html("");
}

$(".image-upload-wrap").bind("dragover", function () {
  $(this).addClass("image-dropping");
});

$(".image-upload-wrap").bind("dragleave", function () {
  $(this).removeClass("image-dropping");
});

function dropHandler(event) {
  event.preventDefault();
  var files = event.dataTransfer.files;
  $(".file-upload-input")[0].files = files;
  readURL($(".file-upload-input")[0]);
  $(".image-upload-wrap").removeClass("image-dropping");
}

function dragOverHandler(event) {
  event.preventDefault();
  $(".image-upload-wrap").addClass("image-dropping");
}

function dragLeaveHandler(event) {
  $(".image-upload-wrap").removeClass("image-dropping");
}
imgInp.onchange = (evt) => {
  const [file] = imgInp.files;
  if (file) {
    blah.src = URL.createObjectURL(file);
    $("#blah").css("display", "block");
  }
};
