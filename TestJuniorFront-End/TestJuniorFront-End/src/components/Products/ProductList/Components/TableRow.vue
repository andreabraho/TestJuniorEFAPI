<template>
  <div class="row mt-1 on-hover" :class="[index%2==0?'bg-grey':'']" @click="goToDetail()">
        <div class="col-2 namebox">{{item.brandName}}</div>
        <div class="col-4 namebox"><p><b>{{item.name}}</b> | {{item.shortDescription}}</p></div>
        <div class="col-3 ">
            <span v-for="cat in item.categories" 
          :key="cat.id"
          class="text-light text-center  bg-primary rounded-pill me-2 cat-pill "
          data-bs-toggle="tooltip" 
          data-bs-placement="top" 
          :title="cat.name">
           {{cat.name.split(" ")[0]}}  </span>
          
        </div>
        <div class="col-1 "><div class="align-middle"><span class="float-end"><span class="text-success">$</span>{{item.price.toFixed(2)}}</span></div></div>
        <div class="col-2 position-relative">
            
            <div class="position-absolute top-50 start-50 translate-middle">
                <div class="input-group">
                    <button class="btn btn-outline-secondary mybutton position-relative float-end" 
                            @click.stop="goToEdit()">
                <i class="bi bi-pencil-square text-warning position-absolute top-50 start-50 translate-middle" 
                    width="12" 
                    height="12" 
                    viewBox="0 0 12 12"></i>
                </button> 
                <button class=" btn btn-outline-secondary  mybutton position-relative float-end"  
                    @click.stop="deleteProd()"
                    type="button"
                    data-bs-toggle="modal" 
                    data-bs-target="#staticBackdrop">
                    <i class="bi bi-trash-fill text-danger position-absolute top-50 start-50 translate-middle" 
                    width="12" 
                    height="12" 
                    viewBox="0 0 12 12"></i>
                    </button>  
                
                </div>
                
                      
            </div>
        </div>
            
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
    methods:{
        goToDetail(){
            this.$router.push("/products/"+this.item.id)
        },
        goToEdit(){
            this.$router.push("/products/"+this.item.id+"/edit")

        },
        deleteProd(){
            this.$emit("deleteProd",this.item.id)
        },
        substrOnSpace(str){
            str.split(' ')
            return str[0]
        }
    }

}
</script>

<style scoped>
    .bg-grey{
        background-color: rgb(230, 230, 230);
    }
    .on-hover:hover{
        background-color: rgb(216, 216, 216);
    }
    .cat-pill{
        padding: 0px 2px 0px 4px;
        
    }
    .mybutton{
        height: 34px;
        width: 34px;
    }
    .catbox{
        word-wrap: break-word;
        border-radius: 25px;
        font-size: 12px;
    }
    .namebox{
        word-wrap: break-word;

    }
    .center {
    float:none;
    margin-left: auto;
    margin-right: auto;
    }
    .max-w-100{
        max-width: 100px;
    }

</style>