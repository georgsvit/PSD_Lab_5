<template>
  <div class="hello">
    <form class="form-payment" @submit.prevent="onFormSubmit" v-if="showForm">        
        <h1 class="h3 mb-3 fw-normal">Payment form</h1>

        <select v-model="currency" class="form-control">
          <option disabled value="">Choose currency</option>
          <option>UAH</option>
          <option>USD</option>
          <option>EUR</option>
        </select>
        <input class="form-control"          
          v-model="amount" 
          type="number" 
          placeholder="Amount" min="0"/>

        <input class="form-control"          
          v-model="description" 
          type="text" 
          placeholder="Description" />

        <button class="w-100 btn btn-lg btn-primary" type="submit">Pay</button>            
    </form>   

    <form class="form-payment" method="POST" action="https://www.liqpay.ua/api/3/checkout" accept-charset="utf-8" v-if="!showForm">        
        <h1 class="h3 mb-3 fw-normal">Confirm payment</h1>

        <p>Chosen currency: {{currency}}</p>
        <p>Chosen amount: {{amount}}</p>
        <p>Payment description: {{description}}</p>
        
        <input type="hidden" v-model="data" name="data"/>
        <input type="hidden" v-model="signature" name="signature"/>
        <button class="w-100 btn btn-lg btn-success" type="submit">Confirm</button>            
    </form>       
  </div>    
</template>

<script>
export default {
  data() {
    return {
            showForm: true,
            amount: null,   
            currency: "",
            description: "",
            data: "",
            signature: ""
        }
  },
  methods: {    
    onFormSubmit() { 
      this.$http.post('payment/checkout', {'amount': this.amount, 'currency': this.currency, 'description': this.description})
                    .then(response => response.json())
                    .then(json => {
                      console.log(json)
                      if (json != null) {
                        this.data = json.data
                        this.signature = json.sign
                        this.showForm = !this.showForm
                      }
                      console.log(this.data)
                      console.log(this.signature)
                  })        
    },
  }
}
</script>
