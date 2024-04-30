<template>
  <q-dialog v-model="value" @show="getAvatarData">
    <q-card style="min-width: 350px">
      <q-card-section>
        <q-scroll-area style="height: 360px;">
          <q-list separator>
            <q-item v-for="(code, index) in codes" :key="index" clickable :active="current == code"
              @click="current = code">
              <q-item-section>{{ code.label }}</q-item-section>
            </q-item>
            <q-item v-for="(parameter, index) in parameters" :key="index" clickable :active="current == parameter"
              @click="current = parameter">
              <q-item-section>{{ parameter.name }}</q-item-section>
              <q-item-section side>{{ parameter.input.type }}</q-item-section>
            </q-item>
          </q-list>
        </q-scroll-area>
      </q-card-section>
      <q-card-section>
        <q-input :label="$t('value')" v-model="p" />
        <q-chip clickable @click="p=''">clear</q-chip>
        <q-chip clickable @click="p='this.value'">this.value</q-chip>
        <q-chip clickable @click="p='false'">false</q-chip>
        <q-chip clickable @click="p='Math.random()'">Math.random()</q-chip>
      </q-card-section>
      <q-card-actions align="right" class="text-primary">
        <q-btn :disable="!current" flat :label="$t('confirm')" @click="setParam" v-close-popup />
        <q-btn flat :label="$t('cancel')" v-close-popup />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>
<script>
import { defineComponent } from 'vue'
import { api } from '../boot/axios';

export default defineComponent({
  name: 'ParameterDialog',
  data() {
    return {
      codes: [
        {
          label: "Preset: Close eyes",
          code: "this.osc('/tracking/eye/EyesClosedAmount',[1+1e-11])"
        },
        {
          label: "Preset: Chat Date",
          code: "this.chat(new Date(Date.now()).toTimeString(),5)"
        },
        {
          label: "Preset: Voice",
          code: "this.osc('/input/Voice',[1]);this.osc('/input/Voice',[0])"
        }
      ],
      parameters: [],
      avatarData: null,
      p: null,
      current: null
    };
  },
  props: {
    modelValue: {
      type: Boolean
    }
  },
  emits: ["submit", 'update:modelValue'],
  computed: {
    value: {
      get() {
        return this.modelValue;
      },
      set(value) {
        this.$emit('update:modelValue', value);
      }
    }
  },
  methods: {
    getAvatarData() {
      api.request({
        url: "/api/avatar/last"
      }).then((resp) => {
        this.avatarData = resp.data;
        this.parameters = resp.data.parameters.filter(x => x.input);
        if (this.current) {
          for (let i = 0; i < this.parameters.length; i++) {
            if (this.parameters[i].name == this.current.name) {
              this.current = this.parameters[i];
              break;
            }
          }
        }
      });
    },
    setParam() {
      if (this.codes.includes(this.current)) {
        this.setActionCode(this.current.code);
      } else {
        this.setActionParameter(this.current);
      }
    },
    setActionParameter(param) {
      let value = this.p;
      if (!value) {
        if (param.input.type == 'Int') {
          value = 1;
        } else if (param.input.type == 'Float') {
          value = 1e-11;
        } else if (param.input.type == 'Bool') {
          value = true;
        }
      }
      const data = `this.osc('${param.input.address}', [${value}])`;
      this.$emit('submit', data);
    },
    setActionCode(data) {
      this.$emit('submit', data);
    }
  }
})
</script>