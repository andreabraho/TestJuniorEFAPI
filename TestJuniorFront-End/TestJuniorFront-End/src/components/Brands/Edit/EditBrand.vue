<template>
    <div class="row">
        <div class="row">Brand Update</div>
        <div class="row">
            <p v-for="(error,index) in errors" :key="index">{{error}}</p>
        </div>
        <div class="row">
            <div class="col-2"></div>
            <div class="col-8">


            <form @submit.prevent="sendData" >
                <div class="form-group row mt-2">
                <label >Name</label>
                <input type="text" class="form-control" v-model="form.brandName">
                </div>
                <div class="form-group row mt-2">
                    <label > Description</label>
                    <input type="text" class="form-control" v-model="form.description">
                </div>

                <button class="btn btn-outline-primary mt-2 row" >Invio</button>






            </form>

        </div>
        <div class="col-2"></div>




        </div>
        
        






    </div>
</template>


<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const BrandRepository = MyRepositoryFactory.get("brands");
export default ({
    data(){
        return {
            id:0,
            form:{
                id:0,
                brandName:"",
                description:""
            },
            isLoading:true,
            errors:[]
        }
    },
    methods:{
        async load(){
            this.isLoading=true
            this.id=this.$route.params.id
            const { data } = await BrandRepository.getDataForEdit(this.id);
            console.log(data)
            this.form.brandName=data.brandName
            this.form.description=data.description
            this.form.id=data.id
            this.isLoading=false
        },
        async sendData(){
            if(this.checkForm){
                await BrandRepository.editBrand(this.form);
                this.$router.push("/brands/"+this.id)
            }
            
        },
        checkForm(){
            if(this.form.brandName=="")
                this.errors.push("Brand name not valid")
            if(this.form.description=="")
            this.errors.push("Brand description not valid")
            if(this.errors.length>0)
                return false

            return true
        }


    },
    async created(){
        await this.load()
    this.$emit("setActiveLink",2)

    }
})
</script>
