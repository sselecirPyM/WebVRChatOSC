<template>
  <q-form @submit="submit">
    <q-input dense :label="$t('label')" v-model="addbtn.label" />
    <q-input dense :label="$t('icon')" v-model="addbtn.icon">
      <template v-slot:append>
        <q-btn icon="collections">
          <q-popup-proxy cover :breakpoint="600">
            <q-card style="max-width: 600px;">
              <q-btn v-for="icon in listIcons" :key="icon" :icon="icon" @click="selectIcon(icon)" dense flat />
            </q-card>
          </q-popup-proxy>
        </q-btn>
      </template>
    </q-input>
    <q-input dense :label="$t('action')" v-model="addbtn.action" autogrow>
      <template v-slot:append>
        <q-btn icon="code" @click="this.showDialog = true">
        </q-btn>
      </template>
    </q-input>
    <q-input v-if="addbtn.type == 1" dense :label="$t('deactiveAction')" v-model="addbtn.deactiveAction" autogrow>
      <template v-slot:append>
        <q-btn icon="code" @click="this.showDialogD = true">
        </q-btn>
      </template>
    </q-input>
    <q-input v-if="addbtn.type == 2" dense :label="$t('min')" v-model="addbtn.min" type="number" />
    <q-input v-if="addbtn.type == 2" dense :label="$t('max')" v-model="addbtn.max" type="number" />
    <q-btn icon="palette" class="cursor-pointer">
      <q-popup-proxy cover :breakpoint="600">
        <q-card>
          <q-color v-model="addbtn.color" />
          <q-btn :label="$t('clear')" @click="addbtn.color = ''" />
        </q-card>
      </q-popup-proxy>
    </q-btn>
    <q-btn-toggle v-model="addbtn.type" toggle-color="primary" :options="[
      { label: $t('button'), value: 0 },
      { label: $t('toggle'), value: 1 },
      { label: $t('slider'), value: 2 }
    ]" />
    <q-btn flat type="submit" color="blue" :label="$t('submit')" :loading="busy" />
  </q-form>
  
  <ParameterDialog v-model="showDialog" @submit="val => this.addbtn.action = val" />
  <ParameterDialog v-model="showDialogD" @submit="val => this.addbtn.deactiveAction = val" />
</template>

<script>
import { defineComponent } from 'vue'
import { api } from '../boot/axios';
import ListIcons from '../assets/icons'
import ParameterDialog from './ParameterDialog.vue';

export default defineComponent({
  name: 'AddButton',
  data() {
    return {
      addbtn: {
        label: "",
        icon: "",
        action: "",
        deactiveAction: "",
        color: "",
        type: 0,
        category: 0,
        min: 0,
        max: 1
      },
      busy: false,
      listIcons: ListIcons,
      showDialog: false,
      showDialogD: false
    };
  },
  components: {
    ParameterDialog
  },
  emits: ["submit"],
  props: {
    category: {
      type: Number,
      required: true
    }
  },
  mounted() {
  },
  methods: {
    submit() {
      this.busy = true;
      this.addbtn.category = this.category;
      api.request({
        method: "post",
        url: "/api/button/add",
        data: this.addbtn
      }).then(() => this.$emit('submit')).finally(() => {
        this.busy = false;
      });
    },
    selectIcon(icon) {
      this.addbtn.icon = icon;
    }
  }
})
</script>
