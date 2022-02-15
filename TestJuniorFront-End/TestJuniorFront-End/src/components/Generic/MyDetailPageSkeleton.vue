<template>
  <div class="row">
    <div class="row my-5 h2">
      <div class="col-2"></div>
      <div class="col-7">{{ message }} <br><hr></div>
      <div class="col-2"></div>
    </div>  
    
    <div class="row mb-5">
      <div class="col-2"></div>
      <div class="container col-7">
        <div class="skeleton-loading">
          <div class="title-wrapper">
            <div class="circle"></div>
            <div class="content-place-holder"></div>
          </div>
          <div class="inner-wrapper">
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
          </div>
        </div>
        <div class="skeleton-loading">
          <div class="title-wrapper">
            <div class="circle"></div>
            <div class="content-place-holder"></div>
          </div>
          <div class="inner-wrapper">
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
          </div>
        </div>
        <div class="skeleton-loading">
          <div class="title-wrapper">
            <div class="circle"></div>
            <div class="content-place-holder"></div>
          </div>
          <div class="inner-wrapper">
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
          </div>
        </div>
        <div class="skeleton-loading">
          <div class="title-wrapper">
            <div class="circle"></div>
            <div class="content-place-holder"></div>
          </div>
          <div class="inner-wrapper">
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
          </div>
        </div>
        <div class="skeleton-loading">
          <div class="title-wrapper">
            <div class="circle"></div>
            <div class="content-place-holder"></div>
          </div>
          <div class="inner-wrapper">
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
            <div class="content-place-holder"></div>
          </div>
        </div>
      </div>
      <div class="col-3"></div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    message: {
      type: String,
      default: "Detail",
    },
  },
};
</script>

<style>
.container {
  width: 40%;
  border: 1px solid #efefef;
  padding: 10px;
  border-radius: 3px;
  float: left;
  margin-left: 13px;
}
.skeleton-loading {
  border: 1px solid;
  height: 100px;
  border: 1px solid #efe9e9;
  box-shadow: 0px 0px 3px rgba(0, 0, 0, 0.1);
  box-sizing: border-box;
  margin-bottom: 10px;
}
.title-wrapper {
  width: 100%;
  overflow: hidden;
}
.inner-wrapper {
  width: 100%;
  height: auto;
  overflow: hidden;
  float: left;
}
.inner-wrapper .content-place-holder {
  width: calc(50% - 21px);
  margin: 5px 10px;
}

.title-wrapper .circle {
  float: left;
  width: 25px;
  height: 25px;
  background-color: #ccc;
  border-radius: 25%;
  margin: 8px;
  background-image: linear-gradient(90deg, #ddd 0px, #e8e8e8 40px, #ddd 80px);
  background-size: 600px;
  animation: shine-circle 1.6s infinite linear;
}
.title-wrapper .content-place-holder,
.inner-wrapper .content-place-holder {
  float: left;
  height: 15px;
  border-radius: 3px;
  background-image: linear-gradient(90deg, #ddd 0px, #e8e8e8 40px, #ddd 80px);
  background-size: 600px;
  animation: shine-lines 1.6s infinite linear;
}
.title-wrapper .circle + .content-place-holder {
  margin-top: 11px;
  width: calc(100% - 121px);
}

@keyframes shine-lines {
  0% {
    background-position: -100px;
  }
  40%,
  100% {
    background-position: 320px;
  }
}
@keyframes shine-circle {
  0% {
    background-position: -32px;
  }
  40%,
  100% {
    background-position: 208px;
  }
}
</style>