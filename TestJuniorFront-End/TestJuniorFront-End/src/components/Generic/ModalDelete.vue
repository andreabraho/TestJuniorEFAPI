<template>
  <div
    class="modal fade"
    id="staticBackdrop"
    data-bs-backdrop="static"
    data-bs-keyboard="false"
    tabindex="-1"
    aria-labelledby="staticBackdropLabel"
    aria-hidden="false"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="staticBackdropLabel">
            Stai Eliminando Un Prodotto
          </h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">Sei sicuro di voler eliminare il prodotto?</div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" data-bs-dismiss="modal">
            Chiudi
          </button>
          <button
            type="button"
            class="btn btn-danger"
            data-bs-dismiss="modal"
            @click.stop="deleteProduct"
          >
            Elimina
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    value: null,
    title: {
      type: String,
      default: "Title",
      required: true,
    },
    bodyMessage: {
      type: String,
      default: "Body Message",
      required: true,
    },
    showModal: {
      type: Boolean,
      required: true,
    },
  },
  methods: {
    showModal() {
        var myModal = new bootstrap.Modal(document.getElementById('myModal'), {
            keyboard: false
            })
        myModal.show()
    },
    hideModal() {
        var myModal = new bootstrap.Modal(document.getElementById('myModal'), {
            keyboard: false
            })
        myModal.hide()
    },
    toggleModal() {
        var myModal = new bootstrap.Modal(document.getElementById('myModal'), {
            keyboard: false
            })
        myModal.toggle()
    },
  },
  watch: {
    showModal() {},
  },
};
</script>

<style>
</style>