<template>
  <div class="row mt-1 on-hover" :class="[index%2==0?'bg-grey':'']">
        <div class="col-2 ">{{item.brandName}}</div>
        <div class="col-5 "><p><b>{{item.name}}</b> | {{item.shortDescription}}</p></div>
        <div class="col-3 ">
          <span v-for="cat in item.categories" 
          :key="cat.id"
          class="rounded-pill bg-primary text-light cat-pill m-1 ">
              {{cat.name}}
          </span>
        </div>
        <div class="col-1 ">${{item.price}}</div>
        <div class="col-1 ">1 2</div>
    </div>
</template>

<script>
export default {
    name:"TableRow",
    props:{
        item:{
            type:Object,
            required:true
        },
        index:Number
    }

}
</script>

<style scoped>
    .bg-grey{
        background-color: rgb(230, 230, 230);
    }
    .on-hover:hover{
        background-color: rgb(219, 217, 217);
    }
    .cat-pill{
        padding: 3px 3px 3px 3px;
    }

</style>