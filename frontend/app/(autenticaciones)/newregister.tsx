import React, { useState } from 'react';
import {
  StyleSheet,
  Text,
  View,
  TextInput,
  TouchableOpacity,
  KeyboardAvoidingView,
  Platform,
  ScrollView,
  Image,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useRouter } from 'expo-router';

export default function NewRegisterScreen() {
  const router = useRouter();
  const [selectedRole, setSelectedRole] = useState('productor');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleCreateAccount = () => {
    console.log('Avanzando al paso 2 para:', email, 'Rol:', selectedRole);
  };

  return (
    <SafeAreaView style={styles.safe} edges={['top', 'left', 'right']}>
      <KeyboardAvoidingView
        style={styles.keyboardView}
        behavior={Platform.OS === 'ios' ? 'padding' : undefined}
      >
        <ScrollView
          contentContainerStyle={styles.scroll}
          showsVerticalScrollIndicator={false}
          keyboardShouldPersistTaps="handled"
        >
          {/* LOGO SUPERIOR */}
          <View style={styles.logoContainer}>
            <Image
              source={require('../../assets/images/logo.png')}
              style={styles.logoImage}
              resizeMode="contain"
            />
          </View>

          {/* SELECTOR DE ROL */}
          <View style={styles.roleSelectorContainer}>
            <TouchableOpacity
              style={[
                styles.roleButton,
                styles.roleButtonLeft,
                selectedRole === 'productor' &&
                  styles.roleButtonActiveProductor,
              ]}
              onPress={() => setSelectedRole('productor')}
              activeOpacity={0.9}
            >
              <Text
                style={[
                  styles.roleButtonText,
                  selectedRole === 'productor' && styles.roleButtonTextActive,
                ]}
              >
                Productor
              </Text>
            </TouchableOpacity>

            <TouchableOpacity
              style={[
                styles.roleButton,
                styles.roleButtonRight,
                selectedRole === 'consumidor' &&
                  styles.roleButtonActiveConsumidor,
              ]}
              onPress={() => setSelectedRole('consumidor')}
              activeOpacity={0.9}
            >
              <Text
                style={[
                  styles.roleButtonText,
                  selectedRole === 'consumidor' && styles.roleButtonTextActive,
                ]}
              >
                Consumidor
              </Text>
            </TouchableOpacity>
          </View>

          {/* INDICADOR DE PASOS */}
          <View style={styles.stepperContainer}>
            {/* Paso 1 (Activo) */}
            <View style={[styles.stepCircle, styles.stepCircleActive]}>
              <Text style={styles.stepTextActive}>1</Text>
            </View>

            <View style={[styles.stepLine, styles.stepLineActive]} />

            {/* Paso 2 (Inactivo) */}
            <View style={styles.stepCircle}>
              <Text style={styles.stepText}>2</Text>
            </View>

            <View style={styles.stepLine} />

            {/* Paso 3 (Inactivo) */}
            <View style={styles.stepCircle}>
              <Text style={styles.stepText}>3</Text>
            </View>
          </View>

          {/* FORMULARIO */}
          <View style={styles.formContainer}>
            <TextInput
              style={styles.input}
              placeholder="Correo Electrónico"
              placeholderTextColor="#9CA3AF"
              value={email}
              onChangeText={setEmail}
              keyboardType="email-address"
              autoCapitalize="none"
            />

            <TextInput
              style={styles.input}
              placeholder="Contraseña"
              placeholderTextColor="#9CA3AF"
              value={password}
              onChangeText={setPassword}
              secureTextEntry
              autoCapitalize="none"
            />

            <TouchableOpacity style={styles.forgotPasswordContainer}>
              <Text style={styles.forgotPasswordText}>
                ¿Olvidó su contraseña?
              </Text>
            </TouchableOpacity>

            {/* BOTÓN CREAR CUENTA */}
            <TouchableOpacity
              style={styles.createButton}
              onPress={handleCreateAccount}
              activeOpacity={0.8}
            >
              <Text style={styles.createButtonText}>Crear Cuenta</Text>
            </TouchableOpacity>
          </View>

          {/* ENLACE PARA REGRESAR AL LOGIN */}
          <TouchableOpacity
            style={styles.backToLoginContainer}
            onPress={() => router.back()}
          >
            <Text style={styles.backToLoginText}>
              Ya tengo cuenta. Iniciar sesión
            </Text>
          </TouchableOpacity>
        </ScrollView>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  safe: {
    flex: 1,
    backgroundColor: '#FFFFFF',
  },
  keyboardView: {
    flex: 1,
  },
  scroll: {
    flexGrow: 1,
    paddingHorizontal: 30,
    paddingTop: 40,
    paddingBottom: 30,
    alignItems: 'center',
  },
  logoContainer: {
    marginBottom: 30,
    alignItems: 'center',
    justifyContent: 'center',
    width: '100%',
    backgroundColor: '#1A2E2B',
    paddingVertical: 20,
    borderRadius: 24,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.15,
    shadowRadius: 10,
    elevation: 5,
  },
  logoImage: {
    width: 220,
    height: 160,
  },
  roleSelectorContainer: {
    flexDirection: 'row',
    width: '100%',
    height: 45,
    marginBottom: 25,
  },
  roleButton: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#E5E7EB',
  },
  roleButtonLeft: {
    borderTopLeftRadius: 25,
    borderBottomLeftRadius: 25,
  },
  roleButtonRight: {
    borderTopRightRadius: 25,
    borderBottomRightRadius: 25,
  },
  roleButtonActiveProductor: {
    backgroundColor: '#45C264',
  },
  roleButtonActiveConsumidor: {
    backgroundColor: '#E87722',
  },
  roleButtonText: {
    fontSize: 15,
    fontWeight: '700',
    color: '#9CA3AF',
  },
  roleButtonTextActive: {
    color: '#FFFFFF',
  },
  stepperContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    width: '80%',
    marginBottom: 30,
  },
  stepCircle: {
    width: 24,
    height: 24,
    borderRadius: 12,
    backgroundColor: '#9CA3AF',
    justifyContent: 'center',
    alignItems: 'center',
  },
  stepCircleActive: {
    backgroundColor: '#E87722',
  },
  stepText: {
    color: '#FFFFFF',
    fontSize: 12,
    fontWeight: 'bold',
  },
  stepTextActive: {
    color: '#FFFFFF',
    fontSize: 12,
    fontWeight: 'bold',
  },
  stepLine: {
    flex: 1,
    height: 2,
    backgroundColor: '#D1D5DB',
    marginHorizontal: 4,
  },
  stepLineActive: {
    backgroundColor: '#FDBA74',
  },
  formContainer: {
    width: '100%',
  },
  input: {
    height: 50,
    borderWidth: 1.5,
    borderColor: '#9CA3AF',
    borderRadius: 12,
    paddingHorizontal: 15,
    fontSize: 15,
    color: '#1A2E2B',
    marginBottom: 15,
    backgroundColor: '#FFFFFF',
  },
  forgotPasswordContainer: {
    alignItems: 'flex-end',
    marginBottom: 25,
  },
  forgotPasswordText: {
    fontSize: 13,
    fontWeight: '700',
    color: '#111827',
  },
  createButton: {
    backgroundColor: '#00A0A5',
    height: 50,
    borderRadius: 25,
    justifyContent: 'center',
    alignItems: 'center',
    elevation: 2,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.2,
    shadowRadius: 4,
  },
  createButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: 'bold',
  },
  backToLoginContainer: {
    marginTop: 30,
  },
  backToLoginText: {
    color: '#6B7280',
    fontSize: 14,
    fontWeight: '600',
  },
});
