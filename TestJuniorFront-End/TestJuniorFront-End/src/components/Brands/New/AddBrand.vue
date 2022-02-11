<template>
  <div v-if="!isLoading" class="row">

    <div class="row mb-3 mt-3">
      <div class="col-2"></div>
      
      <div class="col-8"><p class="h2">Create New Brand</p></div>
      <div class="col-2"></div>
    </div>

    <div class="col-2">

    </div>
    <div class="col-8">
      <form @submit.prevent="sendData">

        <div class="mb-2 mt-5 border" v-if="brandErrors.length>0">
          <p class="err-text"
              v-for="(error,index) in brandErrors"
              :key="index">
              {{error}}
          </p>
        </div>

         <div class="mb-3">
            <label for="exampleInputEmail1" class="form-label">Email address</label>
            <input type="email" 
                  class="form-control" 
                  aria-describedby="emailHelp"
                  v-model="form.account.email"
                  @blur="validateEmail">
            <div id="emailHelp" class="form-text">We'll never share your email with anyone else.</div>
          </div>
          <div class="mb-3">
            <label for="exampleInputPassword1" class="form-label">Password</label>
            <input type="password" 
                  class="form-control" 
                  v-model="form.account.password">
          </div>
          <div class="mb-3">
            <label for="exampleFormControlInput1" class="form-label">Brand Name</label>
            <input type="text" class="form-control"  v-model="form.brand.brandName">
          </div>
          <div class="mb-3">
            <label for="exampleFormControlTextarea1" class="form-label">Brand Description</label>
            <textarea class="form-control"  rows="3" v-model="form.brand.description"></textarea>
          </div>
          



          <!-- ---------------------------------------------------------------------------------- -->
          
          
          <div v-for="(product,index) in form.prodsWithCats"
                class="border bg-light mt-5"
                :key="index"
                >

          <div>
            <p class="h3 mt-2 mb-3 h-25 position-relative"> 
              Product #{{index+1}}
              <i class="btn btn-danger bi bi-file-earmark-x position-absolute top-0 end-0  me-2"
              @click="removeProduct(index)"></i></p>
          </div>

          <div v-if="prodErrors[index]!=undefined ">

            <p class="h6 err-text" 
                style="white-space: pre;">
              {{prodErrors[index]!=''?prodErrors[index]:""}}
            </p>

          </div>

          <div class="form-group row mt-2">
            <label >Name</label>
            <input type="text" 
                  class="form-control" 
                  v-model="product.product.name">
            </div>
            <div class="form-group row mt-2">
                <label >Short Description</label>
                <input type="text" 
                    class="form-control" 
                    v-model="product.product.shortDescription">
            </div>
            
            <div class="form-group row mt-2">
                <label >Description</label>
                <textarea class="form-control"  
                        rows="7"
                        v-model="product.product.description"></textarea>
            </div>
            <div class="form-group row mt-2">
                <label >Price</label>
                <input type="number" 
                      class="form-control" 
                      v-model="product.product.price" step="any">
            </div>
            
            <!--
            <div class="row mt-2">
                <label for="">Categories</label>
                <select class="selectpicker " 
                        multiple  
                        v-model="product.categoriesIds"
                        size="10">

                    <option 
                    v-for="item in catForSelect"
                    :key="item.id"
                    :value="item.id"
                    selected>{{item.name}}
                    </option>
                
                </select>
            </div>
            -->


            <div class="row ">
              <div class="m-3 checboxes">
                  <div class="form-check form-check-inline col mt-3"
                  v-for="item in catForSelect"
                  :key="item.id">
                    <input class="form-check-input" type="checkbox" :value="item.id" v-model="product.categoriesIds">
                    <label class="form-check-label" >{{item.name}}</label>
                  </div>
              </div>
              
            </div>

          </div>
          
          <div class="mt-5 mb-5 ">
          
            <button type="submit" id="submitBtn" class="btn btn-primary ms-3 float-end">Submit</button>
          </div>
          









    </form>
    
    </div>
    <div class="col-2">
        <button  class="btn btn-warning float-end sticky-top addProdBtn" @click.stop="addProduct">Add Product</button>

    </div>
    <div class="row" id="end"></div>

    

  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
const BrandRepository = MyRepositoryFactory.get("brands");

export default {
  data(){
    return {
      id:0,
      form:{
        account:{
          email:"",
          password:"",
        },
        brand:{
          brandName:"",
          description:"",
        },
        prodsWithCats:[]
      },
      catForSelect:null,
      isLoading:false,
      brandErrors:[],
      prodErrors:[]
    }
  },
  methods:{
    /**loead needed data at creation of the page */
    async load(){
      this.isLoading=true
      const {data} = await ProductRepository.getDataForCreate();
      this.catForSelect = data.categories;
      this.isLoading=false

    },
    /**sends data to api if form is correct */
    async sendData(){
      if(await this.formCheck()){
        const {data} =await BrandRepository.createBrand(this.form)
        this.$router.push("/brands/" + data);

      }
      
    },
    async addProduct(){
      await this.form.prodsWithCats.push({product:{name:"",shortDescription:"",description:"",price:0},categoriesIds:[]})

      let elmnt = document.getElementById('submitBtn');
      elmnt.scrollIntoView(true);
    },
    removeProduct(index){
      this.form.prodsWithCats.splice(index,1)
    },
    /**check if data on form are correct 
     **return true or false
     */
    async formCheck(){
      /** reset errors */
      this.prodErrors=[]
      this.brandErrors=[]

      if(await this.validateEmail()){
        
        if (this.form.account.email.length==0) {
        this.brandErrors.push("Not valid email"); 
        }
        if(this.form.account.password.length<1 || this.form.account.password.length>18)
          this.brandErrors.push("Password must be between 1 and 17 characters"); 
        if(this.form.brand.brandName.length==0){
          this.brandErrors.push("Insert brand name")
        }
        /**fill the prod error array if needed */
        this.calculateProdErrors()

        if(this.brandErrors.length>0 || this.checkIfProdsHaveErrors()){//if there are brand error or prod have errors
          return false
        }
        return true
      }else{
        return false;
      }
      
      
    },
    /** inser in the prod error array the errors of the form */
    calculateProdErrors(){
        
      for(let i=0;i<this.form.prodsWithCats.length;i++){
        /**error string for each product */
        let s="\n"
        if (this.form.prodsWithCats[i].categoriesIds.length <= 0)
          s+="Select at least one category \n";

        if (this.form.prodsWithCats[i].product.name == "")
          s+="Product name not valid \n";

        if (this.form.prodsWithCats[i].product.shortDescription == "") {
          s+="Short description not valid \n";
        }
      this.prodErrors.push(s)

      }
    },
    /**check if the array of product errors have errors or default values */
    checkIfProdsHaveErrors(){
      for(let i=0;i<this.prodErrors.length;i++)
        if(this.prodErrors[i]!="\n")
          return true
        return false
    },
    /**tells if email is valid true if is valid false if not */
    async validateEmail(){
      this.brandErrors=[]
       const {data} =await BrandRepository.validateEmail(this.form.account.email)
       if(!data){
         this.brandErrors.push("Email already exists")
         return false
       }
       return data   
    }
      
  },
  async created(){
    this.$emit("setActiveLink",2)
    await this.load()
  }

}
</script>

<style scoped>
  .bg-light{
    background-color: azure;
    border-top: 2px solid black  !important;;
  }
  .addProdBtn{
    
  position: sticky;
  top: 60px;

  }
  .err-text{
    color: rgb(84, 2, 2);
    font-size: 12px;
  }
  .bounce-enter-active {
  animation: bounce-in .5s;
  }
  .bounce-leave-active {
    animation: bounce-in .5s reverse;
  }
  .fade-enter-active, .fade-leave-active {
  transition: opacity .5s
  }
  .fade-enter, .fade-leave-to /* .fade-leave-active below version 2.1.8 */ {
    opacity: 0
  }
@keyframes bounce-in {
  0% {
    transform: scale(0);
  }
  50% {
    transform: scale(1.1);
  }
  100% {
    transform: scale(1);
  }
}
</style>