const Confirm = require('prompt-confirm');
const execa = require('execa');

(async () => {
  const answer = await new Confirm('Would you like to push?').run().then(answer => answer);

  const remote = execa.sync('git', ['remote']).stdout;
  const branch = execa.sync('git', ['rev-parse', '--abbrev-ref', 'HEAD']).stdout;

  if (answer) {
    try {
      await execa('git', ['push', remote, branch], { stdout: 'inherit' });
      await execa('git', ['fetch', remote], { stdout: 'inherit' });
      console.log('Successfully!');
    } catch (error) {
      console.log('An error occured.' + error);
    }
  }
})();
