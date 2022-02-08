<template>
  <div class="row mt-1 on-hover" :class="[index%2==0?'bg-grey':'']" @click="goToDetail(item.id)">
        <div class="col-1">{{item.id}}</div>
        <div class="col-2 ">{{item.brandName}}</div>
        <div class="col-2 "><p> {{item.productName}}</p></div>
        <div class="col-2 ">
          {{item.user.name}} {{item.user.lastName}}
        </div>
        <div class="col-3 ">{{item.requestText}}</div>
        <div class="col-2 ">{{data}}</div>

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
    },
    computed:{
        /**comuted propriety to have only the date
         *  of the datetime coming from api */
        data(){
            let data=this.item.insertDate.split("T")
            return data[0]
        }
    },
    methods:{
        /**reidiret to detail of the lead */
        goToDetail(productId){
            this.$router.push("/leads/"+productId)
        }
    }

}
</script>

<style scoped>
    .bg-grey{
        background-color: rgb(230, 230, 230);
    }
    .on-hover:hover{
        background-color: rgb(236, 234, 234);
    }
    .cat-pill{
        padding: 1px 3px 0px 3px;
    }

</style>