package cmd

import (
	"bytes"
	"fmt"
	"os"
	"text/template"
)

func executeTemplate(tmplStr string, data interface{}) ([]byte, error) {
	tmpl, err := template.New("").Parse(tmplStr)
	if err != nil {
		return nil, err
	}

	buf := new(bytes.Buffer)
	err = tmpl.Execute(buf, data)
	return buf.Bytes(), err
}

func er(msg interface{}) {
	fmt.Println("Error:", msg)
	os.Exit(1)
}
