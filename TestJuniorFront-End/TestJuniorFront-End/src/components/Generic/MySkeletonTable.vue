<template>
  <!--eslint-disable no-mixed-spaces-and-tabs -->
  <div>
    <div class="row h2 my-5 ms-1">
      <div class="">{{message}}</div>
    <hr>
    </div>
    <div class="row">
      <table cellpadding="0" cellspacing="0" width="100%" class="table_loader">
        <tr>
          <th class="col1">
            <span></span>
          </th>
          <th class="col4">
            <span></span>
          </th>
          <th class="col5">
            <span></span>
          </th>
        </tr>
        <tr>
          <td class="col1">
            <span></span>
          </td>
          <td class="col4">
            <span></span>
            <span class="sub-temp"></span>
            <span class="sub-temp sub-temp-three"></span>
          </td>
          <td class="col5">
            <span></span>
          </td>
        </tr>
        <tr>
          <td class="col1">
            <span></span>
          </td>
          <td class="col4">
            <span></span>
            <span class="sub-temp"></span>
            <span class="sub-temp sub-temp-three"></span>
          </td>
          <td class="col5">
            <span></span>
          </td>
        </tr>
        <tr>
          <td class="col1">
            <span></span>
          </td>
          <td class="col4">
            <span></span>
            <span class="sub-temp"></span>
            <span class="sub-temp sub-temp-three"></span>
          </td>
          <td class="col5">
            <span></span>
          </td>
        </tr>
        <tr>
          <td class="col1">
            <span></span>
          </td>
          <td class="col4">
            <span></span>
            <span class="sub-temp"></span>
            <span class="sub-temp sub-temp-three"></span>
          </td>
          <td class="col5">
            <span></span>
          </td>
        </tr>
        <tr>
          <td class="col1">
            <span></span>
          </td>
          <td class="col4">
            <span></span>
            <span class="sub-temp"></span>
            <span class="sub-temp sub-temp-three"></span>
          </td>
          <td class="col5">
            <span></span>
          </td>
        </tr>
        <tr>
          <td class="col1">
            <span></span>
          </td>
          <td class="col4">
            <span></span>
            <span class="sub-temp"></span>
            <span class="sub-temp sub-temp-three"></span>
          </td>
          <td class="col5">
            <span></span>
          </td>
        </tr>
      </table>
    </div>
  </div>
</template>

<script>
/* eslint-disable no-mixed-spaces-and-tabs */

export default {
  props:{
    message:{
      type:String,
      default:"Page"
    }
  }
};
</script>
 
<style>
.table_loader {
  background-color: #fff;
  color: #666;
  width: 100%;
  padding: 0;
}
.table_loader th {
  background: #f7f9fa;
  border-top: 1px solid #e5e5e5;
  border-bottom: 1px solid #e5e5e5;
  padding: 12px 10px;
  text-align: left;
}
.table_loader td {
  border-top: 0px solid #e5e5e5;
  border-bottom: 1px solid #e5e5e5;
  padding: 12px 10px;
  vertical-align: top;
}
.table_loader span {
  width: calc(100% - 5px);
  height: 15px;
  border-radius: 3px;
  background-image: linear-gradient(90deg, #ddd 0px, #e8e8e8 40px, #ddd 80px);
  background-size: 600px;
  animation: shine-lines-table 1.6s infinite linear;
  display: inline-block;
  min-width: 15px;
}
.table_loader span.sub-temp {
  width: calc(100% - 50%);
  margin-top: 5px;
}
.table_loader span.sub-temp-three {
  width: calc(100% - 70%);
  display: block;
}
.col1 {
  width: 25px;
}
.col2 {
  width: 60px;
}
.col3 {
  width: 45px;
}
.col4 {
  width: 344px;
}
.col5 {
  width: 145px;
}
.col6 {
  width: 113px;
}
.col7 {
  width: 73px;
}
.col8 {
  width: 160px;
}
.col9 {
  width: 92px;
}

@keyframes shine-lines-table {
  0% {
    opacity: 0.4;
  }
  40% {
    opacity: 0.8;
  }
  100% {
    opacity: 0.4;
  }
}
</style>