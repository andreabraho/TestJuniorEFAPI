
</script>
<template>
  <div class="row" v-if="!isLoading">
      <div class="col-1"></div>
      <div class="col-7">
          <div class="d-flex justify-content-start mt-3"> 
            <span class="h2 ">{{mainMessage}}</span>
          </div>

          <form @submit.prevent="sendData" class="mt-5 mb-5">
            <div class="form-group row mt-2">
                <label >Name</label>
                <input type="text" class="form-control" v-model="form.product.name">
            </div>
            <div class="form-group row mt-2">
                <label >Short Description</label>
                <input type="text" class="form-control" v-model="form.product.shortDescription">
            </div>
            
            <div class="form-group row mt-2">
                <label >Description</label>
                <textarea class="form-control"  
                        rows="7"
                        v-model="form.product.description"></textarea>
            </div>
            <div class="form-group row mt-2">
                <label >Price</label>
                <input type="number" class="form-control" v-model="form.product.price">
            </div>
            
            <div v-if="id==0" class="row form-group mt-2">
                <label for="">Brand</label>
                <select class="form-select" v-model="form.product.brandId">
                <option v-for="item in brandForSelect" 
                        :key="item.id"
                        :value="item.id">{{item.name}}</option>
                </select>
            </div>

            <div class="row mt-2">
                <label for="">Categories</label>
                <select class="selectpicker " 
                        multiple  
                        v-model="form.categoriesIds"
                        size="10">

                    <option 
                    v-for="item in catForSelect"
                    :key="item.id"
                    :value="item.id"
                    selected>{{item.name}}
                    </option>
                
                </select>
            </div>

            <div class="d-flex justify-content-end row mt-2">
            <button class="btn btn-outline-primary mt-2 " >Invio</button>
            </div>
        </form>

      </div>
      <div class="col-4 "></div>
    
  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
export default {
    data(){
        return{
            form:{
                product:{
                    id:0,
                    name:"",
                    shortDescription:"",
                    description:"",
                    price:0,
                    brandId:0
                },
                categoriesIds:[],
                
            },
            isLoading:false,
            id:this.$route.params.id,
            catForSelect:null,
            brandForSelect:null,
            mainMessage:"",
        }
    },
    methods:{
        async load(){
            this.isLoading=true
            if(this.$route.params.id!=undefined)
                this.id=this.$route.params.id
            else
                this.id=0
            let  data 
            if(this.id!=0 ){
                data  = await ProductRepository.getDataForUpdate(this.id);
            }
            else{
                data  = await ProductRepository.getDataForCreate();

            }
            this.updateFormData(data.data)
        },
        async sendData(){
            
            if(this.id!=0)
                await ProductRepository.editProduct(this.form);
            else
                this.idProduct=await ProductRepository.createProduct(this.form);

            this.$router.push("/products/"+this.idProduct)
        },
        updateFormData(data){
            if(this.id!=0){
                this.form.product.name=data.product.name
                this.form.product.shortDescription=data.product.shortDescription
                this.form.product.description=data.product.shortDescription
                this.form.product.price=data.product.price
                this.form.product.brandId=data.product.brandId

                this.form.categoriesIds=data.categoriesAssociated
                this.catForSelect=data.allCategories
                this.brandForSelect=data.allBrands
                this.mainMessage="Update Product"
            }else{
                this.catForSelect=data.categories
                this.brandForSelect=data.brands
                this.mainMessage="Create Product"
            }
            this.isLoading=false
            

        }
    },
    async created(){
        await this.load()
    }
}
</script>

<style scoped>
.myselect{
    height: 200px;
}
</style>