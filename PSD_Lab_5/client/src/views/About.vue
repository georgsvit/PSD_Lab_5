<template>
  <div class="about">
    <h1>Thanks for payment!</h1>

    <b-table class="table" :items="payments" :fields="columns">
      <template #cell(orderId)="data">
          <span>{{ data.item.orderId }}</span>
      </template>

      <template #cell(amount)="data">
          <span>{{ data.item.amount }}</span>
      </template>

      <template #cell(currency)="data">
          <span>{{ data.item.currency }}</span>
      </template>

      <template #cell(description)="data">
          <span>{{ data.item.description }}</span>
      </template>
      
      <template #cell(status)="data">
          <span>{{ data.item.status }}</span>
      </template>      
    </b-table>
  </div>
</template>

<script>
export default {
  data() {
    return {
     payments: [],
     columns: [
        {
          key: 'orderId',
          label: 'Id'
        },
        {
          key: 'amount',
          label: 'Amount'
        },
        {
          key: 'currency',
          label: 'Currency'
        },
        {
          key: 'description',
          label: 'Description'
        },
        {
          key: 'status',
          label: 'Status'
        }
      ]
    }
  },
  created() {
    this.$http.get('payment/payments')
      .then(response => response.json())
      .then(json => {
        this.payments = json.map(payment => ({
          'amount': payment.amount,
          'currency': payment.currency,              
          'description': payment.description,
          'orderId': payment.orderId,
          'status': payment.status,
      }))
    })        
  }
}
</script>
