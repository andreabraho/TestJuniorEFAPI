<template>
    <tr class="on-hover" @click="goToDetail(brand.id)"  >
        <th scope="row">{{brand.id}}</th>
        <td>{{brand.name}}</td>
        <td>{{brand.description}}</td>
        <td>
            <div>
            <button class="btn btn-outline-secondary " @click.stop="goToEdit()">
                <i class="bi bi-pencil-square text-warning" 
                    width="16" 
                    height="16" 
                    viewBox="0 0 16 16"></i>
                </button> 
            <button class=" btn btn-outline-secondary " 
                    @click.stop="deleteBrand()"
                    type="button"
                    data-bs-toggle="modal" 
                    data-bs-target="#staticBackdrop">
                    <i class="bi bi-trash-fill text-danger" 
                    width="16" 
                    height="16" 
                    viewBox="0 0 16 16"></i>
                    </button></div>
        </td>
    </tr>
</template>

<script>

export default {
    name:"BrandRow", 
    props:{
        /**brand subject of the row */
        brand:{
            type:Object,
            required:true
        }
    },
    methods:{
        /**method to go to brand detail page */
        goToDetail(brandId){
            this.$router.push("/brands/"+brandId)
        },
        deleteBrand(){
            this.$emit("deleteBrand",this.brand.id)
        },
        goToEdit(){
            this.$router.push("/brands/"+this.brand.id+"/edit")

        }
    }

}
</script>

<style>
.on-hover:hover{
        background-color: rgb(236, 234, 234);
    }
</style>