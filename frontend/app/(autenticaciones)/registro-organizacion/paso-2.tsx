import React, { useState } from 'react';
import {
  StyleSheet,
  Text,
  View,
  TouchableOpacity,
  ScrollView,
  TextInput,
  KeyboardAvoidingView,
  Platform,
  Image,
  KeyboardTypeOptions,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useRouter } from 'expo-router';
import {
  ArrowLeft,
  User,
  Phone,
  Upload,
  Lock,
  Check,
} from 'lucide-react-native';

// --- DEFINICIÓN DE TIPOS ---
interface FormInputProps {
  label?: string;
  initialValue?: string; // Agregamos esto para pre-llenar los datos
  icon?: any;
  multiline?: boolean;
  showCount?: boolean;
  keyboardType?: KeyboardTypeOptions;
}

interface UploadBoxProps {
  label: string;
  subLabel: string;
}

// --- COMPONENTES REUTILIZABLES ---

const FormInput = ({
  label,
  initialValue = '',
  icon: Icon,
  multiline = false,
  showCount = false,
  keyboardType = 'default',
}: FormInputProps) => {
  const [value, setValue] = useState(initialValue);
  const maxChars = 150;

  return (
    <View style={styles.inputGroup}>
      {label && <Text style={styles.label}>{label}</Text>}
      <View
        style={[
          styles.inputContainer,
          multiline && styles.inputContainerMultiline,
        ]}
      >
        {Icon && (
          <View style={styles.iconWrapper}>
            <Icon color="#45C264" size={20} />
          </View>
        )}
        <TextInput
          style={[styles.input, multiline && styles.inputMultiline]}
          value={value}
          onChangeText={(text) => {
            if (showCount && text.length > maxChars) return;
            setValue(text);
          }}
          multiline={multiline}
          keyboardType={keyboardType}
        />
      </View>
      {showCount && (
        <Text style={styles.charCount}>{`${value.length}/${maxChars}`}</Text>
      )}
    </View>
  );
};

const UploadBox = ({ label, subLabel }: UploadBoxProps) => (
  <View style={styles.inputGroup}>
    <Text style={styles.label}>{label}</Text>
    <TouchableOpacity style={styles.uploadContainer}>
      <Upload color="#45C264" size={20} style={styles.uploadIcon} />
      <View>
        <Text style={styles.uploadTitle}>
          Subir {label.includes('Logo') ? 'logo' : 'Documento'}
        </Text>
        <Text style={styles.uploadSubtitle}>{subLabel}</Text>
      </View>
    </TouchableOpacity>
  </View>
);

// --- PANTALLA PRINCIPAL ---

export default function RegisterOrgStep2Screen() {
  const router = useRouter();

  return (
    <SafeAreaView style={styles.safeArea}>
      <KeyboardAvoidingView
        behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
        style={styles.keyboardView}
      >
        <View style={styles.header}>
          <TouchableOpacity
            onPress={() => router.back()}
            style={styles.backButton}
          >
            <ArrowLeft color="#1A2E2B" size={24} />
          </TouchableOpacity>
        </View>

        <ScrollView
          showsVerticalScrollIndicator={false}
          contentContainerStyle={styles.scrollContent}
        >
          <View style={styles.introSection}>
            <View style={styles.logoCircle}>
              <Image
                source={require('../../../assets/images/logo.png')}
                style={styles.logoImage}
                resizeMode="contain"
              />
            </View>
            <Text style={styles.mainTitle}>Acceso institucional</Text>
            <Text style={styles.mainSubtitle}>Un paso más</Text>
          </View>

          {/* Barra de Progreso Actualizada para el Paso 2 */}
          <View style={styles.progressSection}>
            <Text style={styles.sectionTitle}>
              Validar información de Institución
            </Text>
            <Text style={styles.sectionSubtitle}>
              verifica los datos de su organización
            </Text>

            <View style={styles.progressBar}>
              {/* Paso 1: Completado (Check verde) */}
              <View style={[styles.stepCircle, styles.stepCompleted]}>
                <Check color="#FFFFFF" size={16} strokeWidth={3} />
              </View>
              {/* Línea conectora verde */}
              <View style={[styles.stepLine, styles.stepLineActive]} />
              {/* Paso 2: Activo */}
              <View style={[styles.stepCircle, styles.stepActive]}>
                <Text style={styles.stepTextActive}>2</Text>
              </View>
            </View>
          </View>

          <View style={styles.formSection}>
            <FormInput label="Nombre Institución" initialValue="INTA" />
            <FormInput
              label="Tipo de Organización"
              initialValue="Organización"
            />

            <Text style={styles.label}>Nombre del Responsable</Text>
            <View style={{ marginBottom: 16 }}>
              <FormInput
                initialValue="Juan Pablo Albert Einstein"
                icon={User}
              />
              <View style={{ height: 12 }} />
              <FormInput
                initialValue="8918 1005"
                icon={Phone}
                keyboardType="phone-pad"
              />
            </View>

            <View style={styles.row}>
              <View style={styles.halfWidth}>
                <FormInput label="Departamento" initialValue="Jinotega" />
              </View>
              <View style={styles.halfWidth}>
                <FormInput
                  label="Municipio"
                  initialValue="San Sebastián de Yalí"
                />
              </View>
            </View>

            <FormInput label="Dirección" initialValue="BO. Marilú Reyes" />

            <FormInput
              label="Descripción breve"
              initialValue="Siembra y venta de naranjas, limones, limas, toronjas, etc."
              multiline={true}
              showCount={true}
            />

            <UploadBox
              label="Logo de la institución"
              subLabel="PNG o JPG (máx. 2MB)"
            />
            <UploadBox
              label="Documento de verificación"
              subLabel="PDF (máx. 5MB)"
            />

            {/* Botón Final */}
            <TouchableOpacity
              style={styles.submitButton}
              onPress={() => router.push('/registro-organizacion/success')}
            >
              <Text style={styles.submitButtonText}>Crear Cuenta</Text>
            </TouchableOpacity>

            <View style={styles.securityFooter}>
              <Text style={styles.securityText}>
                Tu información está protegida
              </Text>
              <Lock color="#ED8936" size={14} style={{ marginLeft: 6 }} />
            </View>
          </View>
        </ScrollView>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
}

// --- ESTILOS ---

const styles = StyleSheet.create({
  safeArea: { flex: 1, backgroundColor: '#FFFFFF' },
  keyboardView: { flex: 1 },
  header: { paddingHorizontal: 24, paddingTop: 12, paddingBottom: 8 },
  backButton: { width: 40, height: 40, justifyContent: 'center' },
  scrollContent: { paddingBottom: 40 },

  introSection: { alignItems: 'center', marginTop: 10, paddingHorizontal: 24 },
  logoCircle: {
    width: 80,
    height: 80,
    backgroundColor: '#F0FDF4',
    borderRadius: 40,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 16,
  },
  logoImage: { width: 45, height: 45 },
  mainTitle: {
    fontSize: 22,
    fontWeight: '700',
    color: '#2F855A',
    marginBottom: 6,
  },
  mainSubtitle: {
    fontSize: 14,
    color: '#718096',
    textAlign: 'center',
    lineHeight: 20,
  },

  progressSection: {
    alignItems: 'center',
    marginTop: 30,
    paddingHorizontal: 24,
  },
  sectionTitle: {
    fontSize: 15,
    fontWeight: '700',
    color: '#1A2E2B',
    marginBottom: 4,
  },
  sectionSubtitle: { fontSize: 13, color: '#718096', marginBottom: 20 },
  progressBar: {
    flexDirection: 'row',
    alignItems: 'center',
    width: '100%',
    paddingHorizontal: 40,
  },
  stepCircle: {
    width: 28,
    height: 28,
    borderRadius: 14,
    backgroundColor: '#FFFFFF',
    borderWidth: 2,
    borderColor: '#CBD5E0',
    justifyContent: 'center',
    alignItems: 'center',
  },

  // Nuevos estilos de progreso para el Paso 2
  stepCompleted: { backgroundColor: '#45C264', borderColor: '#45C264' },
  stepActive: { backgroundColor: '#45C264', borderColor: '#45C264' },
  stepTextActive: { fontSize: 13, fontWeight: '700', color: '#FFFFFF' },
  stepLine: {
    flex: 1,
    height: 2,
    backgroundColor: '#CBD5E0',
    marginHorizontal: 8,
  },
  stepLineActive: { backgroundColor: '#45C264' },

  formSection: { marginTop: 30, paddingHorizontal: 24 },
  inputGroup: { marginBottom: 18 },
  label: { fontSize: 13, fontWeight: '600', color: '#1A2E2B', marginBottom: 8 },
  inputContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#CBD5E0',
    borderRadius: 10,
    minHeight: 48,
    paddingHorizontal: 14,
  },
  inputContainerMultiline: {
    alignItems: 'flex-start',
    paddingTop: 12,
    minHeight: 100,
  },
  iconWrapper: { marginRight: 10 },
  input: { flex: 1, fontSize: 14, color: '#1A2E2B' },
  inputMultiline: { textAlignVertical: 'top' },
  charCount: {
    fontSize: 11,
    color: '#A0AEC0',
    textAlign: 'right',
    marginTop: 4,
  },

  row: { flexDirection: 'row', justifyContent: 'space-between' },
  halfWidth: { width: '47%' },

  uploadContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    borderWidth: 1.5,
    borderColor: '#45C264',
    borderStyle: 'dashed',
    borderRadius: 10,
    paddingVertical: 16,
  },
  uploadIcon: { marginRight: 10 },
  uploadTitle: { fontSize: 13, fontWeight: '600', color: '#45C264' },
  uploadSubtitle: { fontSize: 11, color: '#A0AEC0', textAlign: 'center' },

  submitButton: {
    backgroundColor: '#45C264',
    borderRadius: 25,
    flexDirection: 'row',
    height: 52,
    justifyContent: 'center',
    alignItems: 'center',
    marginTop: 20,
    marginBottom: 16,
  },
  submitButtonText: { fontSize: 16, fontWeight: '600', color: '#FFFFFF' }, // Sin marginRight porque ya no hay ícono

  securityFooter: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
  },
  securityText: { fontSize: 12, color: '#A0AEC0' },
});
