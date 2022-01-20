import i18n from 'i18n-js';
import { Box, Center, Heading, Text } from 'native-base';
import React from 'react';
import { StyleSheet } from 'react-native';

function HomeScreen() {
  return (
    <Center flex={0.9} px="8">
      <Box
        w={{
          base: '100%',
        }}
      >
        <Heading style={styles.centeredText}> {i18n.t('::Welcome')}</Heading>
        <Text style={styles.centeredText}>
          {i18n.t('::LongWelcomeMessage')}
        </Text>
      </Box>
    </Center>
  );
}

const styles = StyleSheet.create({
  centeredText: {
    textAlign: 'center',
    marginBottom: 5
  },
});

export default HomeScreen;
